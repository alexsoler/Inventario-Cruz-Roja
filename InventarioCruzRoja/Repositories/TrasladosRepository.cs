using AutoMapper;
using InventarioCruzRoja.Data;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Repositories
{
    public class TrasladosRepository : BaseRepository<Traslado, int>, ITrasladosRepository
    {
        private readonly IMapper _mapper;
        private readonly IEgresosRepository _egresosRepository;
        private readonly IIngresosRepository _ingresosRepository;

        public TrasladosRepository(DataContext context, 
            ILogger<TrasladosRepository> logger, 
            IMapper mapper,
            IEgresosRepository egresosRepository,
            IIngresosRepository ingresosRepository) : base(context, logger)
        {
            _mapper = mapper;
            _egresosRepository = egresosRepository;
            _ingresosRepository = ingresosRepository;
        }

        public override async Task<ServiceResponse<IEnumerable<Traslado>>> GetAll(params string[] includes)
        {
            var entidadesResponse = await base.GetAll(includes);

            foreach (var item in entidadesResponse.Data)
            {
                _context.Entry(item.IngresoDestino)
                    .Reference(x => x.Sede)
                    .Load();

                _context.Entry(item.EgresoOrigen)
                    .Reference(x => x.Sede)
                    .Load();
            }
            return entidadesResponse;
        }

        public override async Task<ServiceResponse<Traslado>> Get(object id, params string[] includes)
        {
            var trasladoResponse = await base.Get(id, includes);

            _context.Entry(trasladoResponse.Data.IngresoDestino)
                    .Reference(x => x.Sede)
                    .Load();

            _context.Entry(trasladoResponse.Data.EgresoOrigen)
                .Reference(x => x.Sede)
                .Load();

            return trasladoResponse;
        }

        public async Task<ServiceResponse<Traslado>> AddFromDto(TrasladoDto entity)
        {
            var response = new ServiceResponse<Traslado>();
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var sedeOrigen = await _context.Sedes.FindAsync(entity.SedeOrigenId);
                var sedeDestino = await _context.Sedes.FindAsync(entity.SedeDestinoId);

                var egresoResponse = await _egresosRepository.Add(new Egreso
                {
                    ProductoId = entity.ProductoId,
                    Cantidad = entity.Cantidad,
                    Fecha = DateTime.Now,
                    SedeId = entity.SedeOrigenId,
                    UserId = entity.UserId,
                    Observaciones = $"Egreso debido a traslado hacia la sede: {sedeDestino.Nombre}."
                });

                if (!egresoResponse.Success)
                {
                    await transaction.RollbackAsync();
                    response.Success = false;
                    response.Message = egresoResponse.Message;
                    return response;
                }

                var ingresoResponse = await _ingresosRepository.Add(new Ingreso
                {
                    ProductoId = entity.ProductoId,
                    Cantidad = entity.Cantidad,
                    Fecha = DateTime.Now,
                    SedeId = entity.SedeDestinoId,
                    UserId = entity.UserId,
                    Observaciones = $"Ingreso debido a traslado desde la sede: {sedeOrigen.Nombre}."
                });

                if (!ingresoResponse.Success)
                {
                    await transaction.RollbackAsync();
                    response.Success = false;
                    response.Message = ingresoResponse.Message;
                    return response;
                }

                var traslado = _mapper.Map<Traslado>(entity);

                traslado.IngresoDestinoId = ingresoResponse.Data.Id;
                traslado.EgresoOrigenId = egresoResponse.Data.Id;

                response = await Add(traslado);

                if (response.Success)
                    transaction.Commit();
                else
                    transaction.Rollback();

                return response;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError("Error al registrar el traslado", ex);
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public override async Task<ServiceResponse<Traslado>> Update(Traslado entidad)
        {
            var response = new ServiceResponse<Traslado>();
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var traslado = await _context.Traslados.Include(x => x.IngresoDestino)
                    .Include(x => x.EgresoOrigen).FirstOrDefaultAsync(x => x.Id == entidad.Id);

                traslado.FechaAnula = DateTime.Now;
                traslado.Anulado = true;
                traslado.MotivoAnula = entidad.MotivoAnula;
                traslado.UserAnulaId = entidad.UserAnulaId;

                response = await base.Update(traslado);

                if (!response.Success)
                {
                    await transaction.RollbackAsync();
                    return response;
                }

                traslado.IngresoDestino.Anulado = true;
                traslado.IngresoDestino.FechaAnula = DateTime.Now;
                traslado.IngresoDestino.UserAnulaId = entidad.UserAnulaId;
                traslado.IngresoDestino.MotivoAnula = "El traslado fue anulado.";

                traslado.EgresoOrigen.Anulado = true;
                traslado.EgresoOrigen.FechaAnula = DateTime.Now;
                traslado.EgresoOrigen.UserAnulaId = entidad.UserAnulaId;
                traslado.EgresoOrigen.MotivoAnula = "El traslado fue anulado.";

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return response;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError("Error al anular el traslado", ex);
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
