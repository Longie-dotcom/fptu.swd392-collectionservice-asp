using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Helper;
using Application.Interface.IService;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.IRepository;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Application.Service
{
    public class CollectionTaskService : ICollectionTaskService
    {
        #region Attributes
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        #endregion

        #region Properties
        #endregion

        public CollectionTaskService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteCollectionTaskAsync(Guid collectionTaskId, string deleteBy)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var collectionTask = await unitOfWork.GetRepository<ICollectionTaskRepository>().GetByIdAsync(collectionTaskId);
                if (collectionTask == null)
                {
                    await unitOfWork.RollbackAsync();
                    return (false, "Collection Task not found");
                }
                if (collectionTask.CompletedAt.HasValue)
                {
                    await unitOfWork.RollbackAsync();
                    return (false, "Collection Task đã hoàn thành.");
                }

                var (canDelete, errorMessage) = collectionTask.CheckDeleted();
                if (!canDelete)
                {
                    await unitOfWork.RollbackAsync();
                    return (false, errorMessage);
                }

                // Commit TRƯỚC khi return thành công
                unitOfWork.GetRepository<ICollectionTaskRepository>().Remove(collectionTaskId);
                await unitOfWork.CommitAsync(deleteBy);
                return (true, null);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw; // Ném lại exception để controller xử lý
            }


        }

        public async Task<GenericResult<IEnumerable<CollectionTaskDTO>>> GetAllCollectionTask(string sortBy, QueryCollectionTaskDTO dto)
        {
            var collectionTasks = await GetCollectionTaskWithFilterAsync(
                pageIndex: dto.PageIndex,
                pageSize: dto.PageLength,
                search: dto.Search,
                status: dto.Status,
                sortBy: sortBy);

            var res = mapper.Map<IEnumerable<CollectionTaskDTO>>(collectionTasks);
            return GenericResult<IEnumerable<CollectionTaskDTO>>.Success(res, "Fetch collection tasks successfully");
        }

        public async Task<IEnumerable<CollectionTaskDTO>> GetCollectionTaskWithFilterAsync(
            int pageIndex,
            int pageSize,
            string? search,
            CollectionReportStatus? status,
            string? sortBy = null)
        {
            var entities = await unitOfWork
                .GetRepository<ICollectionTaskRepository>()
                .PagingCollectionTask(search, status, sortBy, pageSize, pageIndex); 
            
            var result = mapper.Map<IEnumerable<CollectionTaskDTO>>(entities);
            return result;
        }

        // Tiếp tục implement trong CollectionTaskService
        public async Task<GenericResult<CollectionTaskDTO>> CreateCollectionTaskAsync(CollectionTaskCreateDTO dto)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();

                // Validate input
                if (string.IsNullOrWhiteSpace(dto.Note))
                    return GenericResult<CollectionTaskDTO>.Failure("Note is required");

                if (dto.AmountEstimated <= 0)
                    return GenericResult<CollectionTaskDTO>.Failure("Amount estimated must be greater than 0");

                // Tạo entity mới
                var collectionTask = new CollectionTask(
                    collectionTaskId: new Guid(),
                    collectionReportId: dto.CollectionReportID,
                    collectorProfileId: dto.CollectorProfileID,
                    note: dto.Note,
                    imageName: dto.ImageName,
                    amountEstimated: dto.AmountEstimated
                );

                unitOfWork.GetRepository<ICollectionTaskRepository>().Add(collectionTask);
                await unitOfWork.CommitAsync();

                var dtoResult = mapper.Map<CollectionTaskDTO>(collectionTask);
                return GenericResult<CollectionTaskDTO>.Success(dtoResult, "Collection task created successfully");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return GenericResult<CollectionTaskDTO>.Failure($"Failed to create collection task: {ex.Message}");
            }
        }

        public async Task<GenericResult<CollectionTaskDTO>> UpdateCollectionTaskAsync(Guid collectionTaskId, UpdateCollectionTaskDTO dto)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();

                var collectionTask = await unitOfWork.GetRepository<ICollectionTaskRepository>().GetByIdAsync(collectionTaskId);
                if (collectionTask == null)
                    return GenericResult<CollectionTaskDTO>.Failure("Collection task not found");

                // Gọi domain method (nó sẽ throw nếu không hợp lệ)
                collectionTask.UpdateDetails(
                    note: dto.Note,
                    imageName: dto.ImageName,
                    amountEstimated: dto.AmountEstimated,
                    collectorProfileId: dto.CollectorProfileID);

                // CHỈ 1 LẦN Update + Commit
                unitOfWork.GetRepository<ICollectionTaskRepository>().Update(collectionTaskId, collectionTask);
                await unitOfWork.CommitAsync();

                var dtoResult = mapper.Map<CollectionTaskDTO>(collectionTask);
                return GenericResult<CollectionTaskDTO>.Success(dtoResult, "Collection task updated successfully");
            }
            catch (InvalidOperationException ex)
            {
                await unitOfWork.RollbackAsync();  // ← THÊM ROLLBACK
                return GenericResult<CollectionTaskDTO>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return GenericResult<CollectionTaskDTO>.Failure($"Failed to update collection task: {ex.Message}");
            }
        }

        public async Task<GenericResult<CollectionTaskDTO>> StartCollectionTaskAsync(Guid collectionTaskId)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();

                var collectionTask = await unitOfWork.GetRepository<ICollectionTaskRepository>().GetByIdAsync(collectionTaskId);
                if (collectionTask == null)
                    return GenericResult<CollectionTaskDTO>.Failure("Collection task not found");

                collectionTask.Start();
                unitOfWork.GetRepository<ICollectionTaskRepository>().Update(collectionTaskId, collectionTask);
                await unitOfWork.CommitAsync();

                var dtoResult = mapper.Map<CollectionTaskDTO>(collectionTask);
                return GenericResult<CollectionTaskDTO>.Success(dtoResult, "Collection task started successfully");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return GenericResult<CollectionTaskDTO>.Failure($"Failed to start collection task: {ex.Message}");
            }
        }

        public async Task<GenericResult<CollectionTaskDTO>> CompleteCollectionTaskAsync(Guid collectionTaskId)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();

                var collectionTask = await unitOfWork.GetRepository<ICollectionTaskRepository>().GetByIdAsync(collectionTaskId);
                if (collectionTask == null)
                    return GenericResult<CollectionTaskDTO>.Failure("Collection task not found");

                collectionTask.Complete();
                unitOfWork.GetRepository<ICollectionTaskRepository>().Update(collectionTaskId, collectionTask);
                await unitOfWork.CommitAsync();

                var dtoResult = mapper.Map<CollectionTaskDTO>(collectionTask);
                return GenericResult<CollectionTaskDTO>.Success(dtoResult, "Collection task completed successfully");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return GenericResult<CollectionTaskDTO>.Failure($"Failed to complete collection task: {ex.Message}");
            }
        }

        public async Task<GenericResult<CollectionTaskDTO>> FailCollectionTaskAsync(Guid collectionTaskId)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();

                var collectionTask = await unitOfWork.GetRepository<ICollectionTaskRepository>().GetByIdAsync(collectionTaskId);
                if (collectionTask == null)
                    return GenericResult<CollectionTaskDTO>.Failure("Collection task not found");

                collectionTask.Fail();
                unitOfWork.GetRepository<ICollectionTaskRepository>().Update(collectionTaskId, collectionTask);
                await unitOfWork.CommitAsync();

                var dtoResult = mapper.Map<CollectionTaskDTO>(collectionTask);
                return GenericResult<CollectionTaskDTO>.Success(dtoResult, "Collection task marked as failed");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return GenericResult<CollectionTaskDTO>.Failure($"Failed to fail collection task: {ex.Message}");
            }
        }

        public async Task<GenericResult<CollectionTaskDTO>> GetCollectionTaskByIdAsync(Guid collectionTaskId)
        {
            var task = await unitOfWork.GetRepository<ICollectionTaskRepository>().GetByIdAsync(collectionTaskId);

            if (task == null)
                return GenericResult<CollectionTaskDTO>.Failure("Collection task not found");

            var dto = mapper.Map<CollectionTaskDTO>(task);
            return GenericResult<CollectionTaskDTO>.Success(dto);
        }

    }
}
