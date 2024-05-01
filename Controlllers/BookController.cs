using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;


namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly BooksService _booksService;

    public BookController(BooksService booksService)
    {
        _booksService = booksService;
    }

    [HttpGet]
    public async Task<List<Book>> Get() =>
        await _booksService.GetBooksAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> Get(string id)
    {
        var book = await _booksService.GetBookAsync(id);

        if (book == null)
        {
            return NotFound();
        }
        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Book book)
    {
        await _booksService.CreateBookAsync(book);

        return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Book bookIn)
    {
        var book = await _booksService.GetBookAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        bookIn.Id = book.Id;

        await _booksService.UpdateBookAsync(id, bookIn);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _booksService.GetBookAsync(id);

        if (book == null)
        {
            return NotFound();
        }
        await _booksService.DeleteBookAsync(id);

        return NoContent();
    }


}
