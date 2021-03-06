﻿using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SwaggerDemo.Entities;
using SwaggerDemo.Services;

namespace SwaggerDemo.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Category[]>> GetAll()
        {
            return await _categoryService.GetAll();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> Get([FromRoute] string id)
        {
            var category = await _categoryService.Get(id);
            
            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Category> Add([FromBody, BindRequired] Category categoryInput)
        {
            var category = _categoryService.Post(categoryInput);
            return CreatedAtAction(nameof(Get), new { category.Id }, category);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> Update([FromRoute] string id, [FromForm, BindRequired] Category categoryInput)
        {
            var category = await _categoryService.Get(id);
            
            if (category == null)
            {
                return NotFound();
            }

            categoryInput.Id = category.Id;
            
            return await _categoryService.Put(categoryInput);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _categoryService.Delete(id);
            return Ok();
        }
    }
}