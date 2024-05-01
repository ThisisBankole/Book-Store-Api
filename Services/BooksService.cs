using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Services;

public class BooksService
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BooksService(IOptions<BookstoreDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(bookStoreDatabaseSettings.Value.BooksCollectionName);
    }
    public async Task<List<Book>> GetBooksAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();
    

    public async Task<Book?> GetBookAsync(string id)=>
        await _booksCollection.Find(book => book.Id == id).FirstOrDefaultAsync();

    
    public async Task CreateBookAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task UpdateBookAsync(string id, Book updatedBook) =>
        await _booksCollection.ReplaceOneAsync(book => book.Id == id, updatedBook);

    public async Task DeleteBookAsync(string id) =>
        await _booksCollection.DeleteOneAsync(book => book.Id == id);


}
