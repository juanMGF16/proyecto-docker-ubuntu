using AutoMapper;
using Business.Repository.Interfaces.Specific.ParametersModule;
using Business.Services.SendEmail.Interfaces;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Specific.ParametersModule;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.ParametersModels.Email;
using Entity.DTOs.ParametersModels.Notification;
using Entity.Models.ParametersModule;
using Microsoft.Extensions.Logging;
using Utilities.Enums.Models;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.ParametersModule
{
    public class NotificationBusiness :
        GenericBusinessDualDTO<Notification, NotificationDTO, NotificationOptionsDTO>,
        INotificationBusiness
    {

        private readonly IGeneral<Notification> _general;
        private readonly INotificationData _notificationData;
        private readonly IEmailService _emailService;

        public NotificationBusiness(
            IGeneral<Notification> general,
            IDataFactoryGlobal factory,
            IEmailService emailService,
            IDeleteStrategyResolver<Notification> deleteStrategyResolver,
            ILogger<Notification> logger,
            IMapper mapper)
            : base(factory.CreateNotificationData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
            _notificationData = factory.CreateNotificationData();
            _emailService = emailService;

        }

        // General 
        public async Task<IEnumerable<NotificationDTO>> GetAllTotalNotificationsAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<NotificationDTO>>(active);
        }

        //Specific
        public async Task<bool> SendEmailNotificationAsync(EmailRequestDTO emailRequest)
        {
            try
            {
                var result = await _emailService.SendEmailAsync(
                    emailRequest.ToEmail,
                    emailRequest.Subject,
                    emailRequest.Body,
                    emailRequest.IsHtml
                );

                if (result)
                {
                    _logger.LogInformation($"Email notification sent to {emailRequest.ToEmail}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email notification to {emailRequest.ToEmail}");
                return false;
            }
        }

        public async Task<bool> SendBulkEmailNotificationAsync(EmailRequestDTO emailRequest)
        {
            try
            {
                var result = await _emailService.SendEmailAsync(
                    emailRequest.ToEmails,
                    emailRequest.Subject,
                    emailRequest.Body,
                    emailRequest.IsHtml
                );

                if (result)
                {
                    _logger.LogInformation($"Bulk email notification sent to {emailRequest.ToEmails.Count} recipients");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending bulk email notification to {emailRequest.ToEmails.Count} recipients");
                return false;
            }
        }

        public async Task LogNotificationAsync(int userId, string title, string content, string type)
        {
            try
            {
                var notification = new Notification
                {
                    Title = title,
                    Content = content,
                    Type = Enum.Parse<TypeNotification>(type),
                    Date = DateTime.UtcNow,
                    Read = false,
                    UserId = userId,
                    Active = true
                };

                await _notificationData.CreateAsync(notification);
                _logger.LogInformation($"Notification logged for user {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error logging notification for user {userId}");
                throw;
            }
        }

        //Actions
        protected override Task BeforeCreateMap(NotificationOptionsDTO dto, Notification entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Title, "Title");
            ValidationHelper.EnsureValidId(dto.UserId, "UserId");

            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(NotificationOptionsDTO dto, Notification entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Title, "Title");

            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(NotificationOptionsDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(f => StringHelper.EqualsNormalized(f.Title, dto.Title)))
                throw new ValidationException("Name", $"Ya existe un Notification con el Titulo '{dto.Title}'.");
        }

        protected override async Task ValidateBeforeUpdateAsync(NotificationOptionsDTO dto, Notification existingEntity)
        {
            var others = await _data.GetAllAsync();

            if (!StringHelper.EqualsNormalized(existingEntity.Title, dto.Title))
            {

                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.Title, dto.Title)))
                    throw new ValidationException("Name", $"Ya existe un Notification con el Titulo ' {dto.Title}'.");
            }
        }
    }
}