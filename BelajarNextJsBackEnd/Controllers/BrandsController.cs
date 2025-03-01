﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BelajarNextJsBackEnd.Entities;
using BelajarNextJsBackEnd.Models;

namespace BelajarNextJsBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BrandsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<List<Brand>>> GetBrands(string? search)
        {
          if (_context.Brands == null)
          {
              return NotFound();
          }

            var query = _context.Brands.AsNoTracking();

            if (string.IsNullOrEmpty(search))
            {
                return await query.ToListAsync();
            }
            return await query
                .Where(Q => Q.Name.ToLower().Contains(search.ToLower()))
                .ToListAsync();
        }

        // GET: api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(string id)
        {
          if (_context.Brands == null)
          {
              return NotFound();
          }
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }

        // PUT: api/Brands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}", Name = "UpdateBrand")]
        public async Task<IActionResult> Post(string id, BrandUpdateModel brand)
        {
            var update = await _context.Brands.Where(Q => Q.Id == id).FirstOrDefaultAsync();
            if (update == null)
            {
                return NotFound();
            }

            //Validasi

            update.Name = brand.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Brands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name = "CreateBrand")]
        public async Task<ActionResult<Brand>> Post(BrandCreateModel brand)
        {
          if (_context.Brands == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Brands'  is null.");
          }

            //Validasi

            var insert = new Brand
            {
                Id = Ulid.NewUlid().ToString(),
                Name = brand.Name,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _context.Brands.Add(insert);


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BrandExists(insert.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return insert;
        }

        // DELETE: api/Brands/5
        [HttpDelete("{id}", Name = "DeleteBrand")]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            if (_context.Brands == null)
            {
                return NotFound();
            }
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BrandExists(string id)
        {
            return (_context.Brands?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
