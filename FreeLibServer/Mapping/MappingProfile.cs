using AutoMapper;
using FreeLibServer.Controllers.Resources;
using FreeLibServer.Core.Models;
using System.Linq;

namespace FreeLibServer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to API resources
            CreateMap<Book, SaveBookResource>()
                .ForMember(br => br.Authors, opt => opt.MapFrom(b => b.Authors.Select(ba => ba.AuthorId)))
                .ForMember(br => br.Genres, opt => opt.MapFrom(b => b.Genres.Select(bg => bg.GenreId)));

            CreateMap<Author, AuthorResource>()
                .ForMember(ar => ar.Books, opt => opt.MapFrom(a => a.Books.Select(ba => ba.BookId)));

            CreateMap<Genre, GenreResource>()
                .ForMember(gr => gr.Books, opt => opt.MapFrom(g => g.Books.Select(bg => bg.GenreId)));

            CreateMap<Book, BookResource>()
                .ForMember(br => br.Authors, opt => opt.MapFrom(b => b.Authors.Select(ba => new AuthorResource { Id = ba.Author.Id, Name = ba.Author.Name } )))
                .ForMember(br => br.Genres, opt => opt.MapFrom(b => b.Genres.Select(bg => new GenreResource { Id = bg.Genre.Id, Name = bg.Genre.Name })));

            //API resources to domain
            CreateMap<SaveBookResource, Book>()
                .ForMember(b => b.Id, opt => opt.Ignore())
                .ForMember(b => b.Authors, opt => opt.Ignore())
                .ForMember(b => b.Genres, opt => opt.Ignore())
                .AfterMap((br, b) => {

                    //удалить авторов и жанры

                    var removedAuthors = b.Authors.Where(a => !br.Authors.Contains(a.AuthorId)).ToList();
                    foreach (var a in removedAuthors)
                        b.Authors.Remove(a);

                    var removedGenres = b.Genres.Where(g => !br.Genres.Contains(g.GenreId)).ToList();
                    foreach (var g in removedGenres)
                        b.Genres.Remove(g);

                    //добавить авторов и жанры

                    var addedAuthors = br.Authors.Where(id => !b.Authors.Any(a => a.AuthorId == id)).Select(id => new BookAuthor { AuthorId = id});
                    foreach (var a in addedAuthors)
                        b.Authors.Add(a);

                    var addedGenres = br.Genres.Where(id => !b.Genres.Any(g => g.GenreId == id)).Select(id => new BookGenre { GenreId = id });

                    foreach (var g in addedGenres)
                        b.Genres.Add(g);

                });

            CreateMap<AuthorResource, Author>()
                .ForMember(a => a.Id, opt => opt.Ignore())
                .ForMember(a => a.Books, opt => opt.Ignore())
                .AfterMap((ar, a) => {

                    var removedBooks = a.Books.Where(b => !ar.Books.Contains(b.BookId)).ToList();
                    foreach (var b in removedBooks)
                        a.Books.Remove(b);

                    var addedBooks = ar.Books.Where(id => !a.Books.Any(b => b.BookId == id)).Select(id => new BookAuthor { BookId = id});
                    foreach (var b in addedBooks)
                        a.Books.Add(b);

                });

            CreateMap<GenreResource, Genre>()
                .ForMember(g => g.Id, opt => opt.Ignore())
                .ForMember(g => g.Books, opt => opt.Ignore())
                .AfterMap((gr, g) => {

                    var removedBooks = g.Books.Where(b => !gr.Books.Contains(b.BookId)).ToList();
                    foreach (var b in removedBooks)
                        g.Books.Remove(b);

                    var addedBooks = gr.Books.Where(id => !g.Books.Any(b => b.BookId == id)).Select(id => new BookGenre { BookId = id});
                    foreach (var b in addedBooks)
                        g.Books.Add(b);

                });

        }
    }
}
