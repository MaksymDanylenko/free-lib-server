﻿using AutoMapper;
using FreeLibServer.Controllers.Resources;
using FreeLibServer.Core.Models;
using System.Linq;

namespace FreeLibServer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //доменная модель -> API ресурсы
            CreateMap<Book, SaveBookResource>()
                .ForMember(br => br.Authors, opt => opt.MapFrom(b => b.Authors.Select(ba => ba.AuthorId)))
                .ForMember(br => br.Genres, opt => opt.MapFrom(b => b.Genres.Select(bg => bg.GenreId)));

            CreateMap<Book, BookResource>()
                .ForMember(br => br.Authors, opt => opt.MapFrom(b => b.Authors.Select(ba => new KeyValuePairResource { Id = ba.Author.Id, Name = ba.Author.Name } )))
                .ForMember(br => br.Genres, opt => opt.MapFrom(b => b.Genres.Select(bg => new KeyValuePairResource { Id = bg.Genre.Id, Name = bg.Genre.Name })));

            CreateMap<Author, KeyValuePairResource>();

            CreateMap<Author, SaveAuthorResource>();

            CreateMap<Author, AuthorResource>()
                .ForMember(ar => ar.Books, opt => opt.MapFrom(a => a.Books.Select(ba => ba.BookId)));

            CreateMap<Genre, KeyValuePairResource>();

            //API ресурсы -> доменная модель

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
                /*.AfterMap((ar, a) => {

                    var removedBooks = a.Books.Where(b => !ar.Books.Contains(b.BookId)).ToList();
                    foreach (var b in removedBooks)
                        a.Books.Remove(b);

                    var addedBooks = ar.Books.Where(id => !a.Books.Any(b => b.BookId == id)).Select(id => new BookAuthor { BookId = id});
                    foreach (var b in addedBooks)
                        a.Books.Add(b);

                })*/;

            CreateMap<SaveAuthorResource, Author>()
                .ForMember(g => g.Id, opt => opt.Ignore());

            CreateMap<KeyValuePairResource, Genre>()
                .ForMember(g => g.Id, opt => opt.Ignore())
                .ForMember(g => g.Books, opt => opt.Ignore());

        }
    }
}
