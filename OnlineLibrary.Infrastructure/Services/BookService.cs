﻿using AutoMapper;
using OnlineLibrary.Application.Interfaces;
using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Domain.Entities.Dtos.Request;
using OnlineLibrary.Domain.Entities.Dtos.Response;
using OnlineLibrary.Infrastructure.Interfaces;

namespace OnlineLibrary.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookResponseDto> AddBook(BookRequestDto newBookDto)
        {
            Book addedBook = await _bookRepository.AddBook(newBookDto);

            return _mapper.Map<BookResponseDto>(addedBook);
        }

        public async Task<BookResponseDto> DeleteBook(int id)
        {
            Book? deletedBook = await _bookRepository.DeleteBook(id);
            return _mapper.Map<BookResponseDto>(deletedBook);
        }

        public async Task<List<BookResponseDto>> GetAllBooks()
        {
            List<Book> books = await _bookRepository.GetAllBooks();           

            return books.Select(book => _mapper.Map<BookResponseDto>(book)).ToList();
        }

        public async Task<List<BookResponseDto>> SearchBooks(SearchBookRequestDto searchBookDto)
        {
            List<Book> books = await _bookRepository.SearchBooks(searchBookDto);

            return books.Select(book => _mapper.Map<BookResponseDto>(book)).ToList();
        }

        public async Task<ServiceResponse<BookResponseDto>> UpdateBook(BookRequestDto bookToUpdateDto, int id)
        {
            var response = new ServiceResponse<BookResponseDto>();
            Book? bookToUpdate = await _bookRepository.UpdateBook(bookToUpdateDto, id);

            if (bookToUpdate is null)
            {
                return await Task.FromResult(new ServiceResponse<BookResponseDto>
                {
                    Message = $"No book found with ID {id}",
                    Success = false
                });
            }

            response.Data = _mapper.Map<BookResponseDto>(bookToUpdate);

            return response;
        }
    }
}
