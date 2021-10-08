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

        public TrasladosRepository(DataContext context, ILogger<TrasladosRepository> logger, IMapper mapper) : base(context, logger)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<Traslado>> AddFromDto(TrasladoDto entity)
        {
            var response = new ServiceResponse<Traslado>();
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var sedeOrigen = await _context.Sedes.FindAsync(entity.SedeOrigenId);
                var sedeDestino = await _context.Sedes.FindAsync(entity.SedeDestinoId);

                var ingresoDestino = _context.Ingresos.Add(new Ingreso
                {
                    ProductoId = entity.ProductoId,
                    Cantidad = entity.Cantidad,
                    Fecha = DateTime.Now,
                    SedeId = entity.SedeDestinoId,
                    UserId = entity.UserId,
                    Observaciones = $"Ingreso debido a traslado desde la sede: {sedeOrigen.Nombre}."
                });
                
                await _context.SaveChangesAsync();

                var egresoOrigen = _context.Egresos.Add(new Egreso
                {
                    ProductoId = entity.ProductoId,
                    Cantidad = entity.Cantidad,
                    Fecha = DateTime.Now,
                    SedeId = entity.SedeOrigenId,
                    UserId = entity.UserId,
                    Observaciones = $"Egreso debido a traslado hacia la sede: {sedeDestino.Nombre}."
                });

                var traslado = _mapper.Map<Traslado>(entity);

                traslado.IngresoDestinoId = ingresoDestino.Entity.Id;
                traslado.EgresoOrigenId = egresoOrigen.Entity.Id;

                await _context.SaveChangesAsync();

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
            entidad.FechaAnula = DateTime.Now;

            var response = await base.Update(entidad);

            var traslado = await _context.Traslados.Include(x => x.IngresoDestino)
                .Include(x => x.EgresoOrigen).FirstOrDefaultAsync(x => x.Id == entidad.Id);

            traslado.IngresoDestino.Anulado = true;
            traslado.IngresoDestino.FechaAnula = DateTime.Now;
            traslado.IngresoDestino.UserAnulaId = entidad.UserAnulaId;
            traslado.IngresoDestino.MotivoAnula = "El traslado fue anulado.";

            traslado.EgresoOrigen.Anulado = true;
            traslado.EgresoOrigen.FechaAnula = DateTime.Now;
            traslado.EgresoOrigen.UserAnulaId = entidad.UserAnulaId;
            traslado.EgresoOrigen.MotivoAnula = "El traslado fue anulado.";

            await _context.SaveChangesAsync();

            return response;
        }
    }
}
