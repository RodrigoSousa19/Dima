﻿using Dima.Api.Data;
using Dima.Core.Common.Extensions;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
    public class TransactionHandler(AppDbContext context) : ITransactionHandler
    {
        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
                request.Amount *= -1;

            try
            {
                var transaction = new Transaction()
                {
                    UserId = request.UserId,
                    CategoryId = request.CategoryId,
                    CreatedAt = DateTime.Now,
                    Amount = request.Amount,
                    PaidOrReceivedAt = request.PaidOrReceivedAt,
                    Title = request.Title,
                    Type = request.Type
                };

                await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, 201, "Transação criada com sucesso!");
            }
            catch
            {

                return new Response<Transaction?>(null, 500, "Não foi possível criar uma nova transação");
            }
        }

        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
        {
            try
            {
                var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                if (transaction is null)
                    return new Response<Transaction?>(null, 404, "Não foi possível encontrar a transação especificada");

                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, message: "Transação excluida com sucesso");
            }
            catch
            {
                return new Response<Transaction?>(null, 500, "Não foi possível deletar a transação especificada");
            }
        }

        public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        {
            try
            {
                var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                return transaction is null
                    ? new Response<Transaction?>(null, 404, "Não foi possível encontrar a transação especificada")
                    : new Response<Transaction?>(transaction);
            }
            catch
            {
                return new Response<Transaction?>(null, 500, "Não foi possível localizar a transação especificada");
            }
        }

        public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
        {
            try
            {
                request.StartDate ??= DateTime.Now.GetFirstDay();
                request.EndDate ??= DateTime.Now.GetLastDay();
            }
            catch
            {

                return new PagedResponse<List<Transaction>?>(null, 500, "Não foi possível determinar a data de inicio ou término.");
            }

            try
            {
                var query = context
                .Transactions
                .AsNoTracking()
                .Where(x => x.PaidOrReceivedAt >= request.StartDate
                && x.PaidOrReceivedAt <= request.EndDate
                && x.UserId == request.UserId)
                .OrderBy(x => x.PaidOrReceivedAt);

                var transactions = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var count = await query.CountAsync();

                return new PagedResponse<List<Transaction>?>(transactions, count, request.PageNumber, request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Transaction>?>(null, 500, "Não foi possível buscar as transações");
            }


        }

        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
                request.Amount *= -1;

            try
            {
                var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (transaction is null)
                    return new Response<Transaction?>(null, 404, "Não foi possível encontrar a transação especificada");

                transaction.CategoryId = request.CategoryId;
                transaction.Amount = request.Amount;
                transaction.Title = request.Title;
                transaction.Type = request.Type;
                transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

                context.Transactions.Update(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, message: "Transação atualizada com sucesso");
            }
            catch
            {
                return new Response<Transaction?>(null, 500, "Não foi possível atualizar a transação especificada");
            }

        }
    }
}
