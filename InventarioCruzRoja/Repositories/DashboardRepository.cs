﻿using InventarioCruzRoja.Data;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioCruzRoja.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<DashboardRepository> _logger;

        public DashboardRepository(DataContext context,
            ILogger<DashboardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResponse<int>> ObtenerCantidadDeProductos()
        {
            var response = new ServiceResponse<int>();
            try
            {
                response.Data = await _context.Productos.Where(x => x.EstadoId == 1).CountAsync();
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener la cantidad de productos", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener la cantidad de productos";

                return response;
            }
        }

        public async Task<ServiceResponse<int>> ObtenerCantidadDeProveedores()
        {
            var response = new ServiceResponse<int>();
            try
            {
                response.Data = await _context.Proveedores.Where(x => x.EstadoId == 1).CountAsync();
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener la cantidad de proveedores", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener la cantidad de proveedores";

                return response;
            }
        }

        public async Task<ServiceResponse<int>> ObtenerCantidadDeSedes()
        {
            var response = new ServiceResponse<int>();
            try
            {
                response.Data = await _context.Sedes.Where(x => x.EstadoId == 1).CountAsync();
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener la cantidad de sedes", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener la cantidad de sedes";

                return response;
            }
        }

        public async Task<ServiceResponse<int>> ObtenerCantidadDeUsuarios()
        {
            var response = new ServiceResponse<int>();
            try
            {
                response.Data = await _context.Users.CountAsync();
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener la cantidad de Usuarios", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener la cantidad de Usuarios";

                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<ResumenEgresosDto>>> ObtenerResumenDeEgresos()
        {
            var response = new ServiceResponse<IEnumerable<ResumenEgresosDto>>();
            try
            {
                var fechaRestar = DateTime.Now.Date;
                var fechas = Enumerable.Range(0, 7).Select(x => fechaRestar.AddDays(-x)).OrderBy(f => f).ToList();
                var fechaDesde = fechas.FirstOrDefault();

                var datos = await _context.Egresos.Where(x => x.Fecha >= fechaDesde && !x.Anulado)
                    .GroupBy(x => x.Fecha.Date)
                    .Select(x => new ResumenEgresosDto
                    {
                        Cantidad = x.Sum(e => e.Cantidad),
                        Fecha = x.Key
                    }).ToListAsync();

                response.Data = fechas.GroupJoin(datos, f => f, e => e.Fecha, (f, e_f) => new { f, e_f })
                    .SelectMany(t => t.e_f.DefaultIfEmpty(), (t, x) => new ResumenEgresosDto
                    {
                        Cantidad = x == null ? 0 : x.Cantidad,
                        Fecha = t.f
                    }).ToList();

                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener el resumen de egresos", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener el resumen de egresos";

                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<ResumenIngresosDto>>> ObtenerResumenDeIngresos()
        {
            var response = new ServiceResponse<IEnumerable<ResumenIngresosDto>>();
            try
            {
                var fechaRestar = DateTime.Now.Date;
                var fechas = Enumerable.Range(0, 7).Select(x => fechaRestar.AddDays(-x)).OrderBy(f => f).ToList();
                var fechaDesde = fechas.FirstOrDefault();

                var datos = await _context.Ingresos.Where(x => x.Fecha >= fechaDesde && !x.Anulado)
                    .GroupBy(x => x.Fecha.Date)
                    .Select(x => new ResumenIngresosDto
                    {
                        Cantidad = x.Sum(i => i.Cantidad),
                        Fecha = x.Key
                    }).ToListAsync();

                response.Data = fechas.GroupJoin(datos, f => f, i => i.Fecha, (f, i_f) => new { f, i_f })
                    .SelectMany(t => t.i_f.DefaultIfEmpty(), (t, x) => new ResumenIngresosDto
                    {
                        Cantidad = x == null ? 0 : x.Cantidad,
                        Fecha = t.f
                    }).ToList();

                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener el resumen de ingresos", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener el resumen de ingresos";

                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<ResumenProductosDto>>> ObtenerResumenDeProductos()
        {
            var response = new ServiceResponse<IEnumerable<ResumenProductosDto>>();
            try
            {
                var fechaRestar = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var fechas = Enumerable.Range(0, 7).Select(x => fechaRestar.AddMonths(-x)).OrderBy(f => f).ToList();
                var fechaDesde = fechas.FirstOrDefault();

                var datos = await _context.Productos.Where(x => x.FechaCreacion >= fechaDesde && x.EstadoId == 1)
                    .GroupBy(x => new { x.FechaCreacion.Month, x.FechaCreacion.Year })
                    .Select(x => new
                    {
                        x.Key.Month,
                        x.Key.Year,
                        Cantidad = x.Count()
                    }).ToListAsync();

                response.Data = fechas.GroupJoin(datos, f => new { f.Month, f.Year }, p => new { p.Month, p.Year }, (f, p_f) => new { f, p_f })
                    .SelectMany(t => t.p_f.DefaultIfEmpty(), (t, x)  => new ResumenProductosDto
                    {
                        Cantidad = x == null ? 0 : x.Cantidad,
                        Anio = t.f.Year,
                        Mes = t.f.Month
                    }).ToList();

                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener el resumen de productos", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener el resumen de productos";

                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<EventoProducto>>> ObtenerUltimosEventos()
        {
            var response = new ServiceResponse<IEnumerable<EventoProducto>>();
            try
            {
                response.Data = await _context.EventosProductos.Include(x => x.Producto).OrderByDescending(x => x.Id).Take(50)
                    .Select(x => new EventoProducto
                    {
                        Descripcion = $"{x.Producto.Nombre}: {x.Descripcion}",
                        Fecha = x.Fecha
                    }).ToListAsync();

                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener los ultimos eventos", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener los ultimos eventos";

                return response;
            }
        }
    }
}
