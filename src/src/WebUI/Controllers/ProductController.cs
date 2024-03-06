using Microsoft.AspNetCore.Mvc;
using src.Application.Product.Command.UpdateProduct;
using src.Application.Product.Queries.GetProducts;
using src.WebUI.Controllers;

namespace WebUI.Controllers;
public class ProductController : ApiControllerBase
{
    [HttpGet("{searchString}")]
    public async Task<ActionResult<List<ProductDto>>> GetProducts(string searchString)
    {
        return await Mediator.Send(new GetProductsQuery { SearchString = searchString });
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
