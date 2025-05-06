using FeedTrac.Server.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FeedTrac.Tests.Helpers
{
    public static class TestDataMocks
    {
        public static ApplicationUser CreateUser(string userId)
        {
            return new ApplicationUser
            {
                Id = userId,
                UserName = $"{userId}@example.com"
            };
        }

        public static Module CreateModule(bool includeStudent = false, bool includeTeacher = false, ApplicationUser user = null)
        {
            var module = new Module
            {
                Id = 1,
                Name = "Sample Module",
                JoinCode = "ABC123",
                StudentModule = new List<StudentModule>(),
                TeacherModule = new List<TeacherModule>(),
                Tickets = new List<FeedbackTicket>()
            };

            if (includeStudent && user != null)
            {
                module.StudentModule.Add(new StudentModule { Module = module, ModuleId = module.Id, User = user, UserId = user.Id });
            }

            if (includeTeacher && user != null)
            {
                module.TeacherModule.Add(new TeacherModule { Module = module, ModuleId = module.Id, User = user, UserId = user.Id });
            }

            return module;
        }

        public static MessageImage CreateValidImageForStudent(string userId)
        {
            var user = CreateUser(userId);
            var module = CreateModule(includeStudent: true, user: user);

            var ticket = new FeedbackTicket
            {
                Module = module,
                ModuleId = module.Id,
                Owner = user,
                OwnerId = user.Id,
                Title = "Student Ticket",
                Status = FeedbackTicket.TicketStatus.Open
            };

            var message = new FeedbackMessage
            {
                Ticket = ticket,
                TicketId = ticket.TicketId,
                Author = user,
                AuthorId = user.Id,
                Content = "Student feedback",
                CreatedAt = DateTime.UtcNow
            };

            return new MessageImage
            {
                Id = 1,
                Message = message,
                MessageId = 1,
                ImageType = "image/jpeg",
                ImageName = "image.jpg",
                ImageData = new byte[] { 1, 2, 3 }
            };
        }

        public static MessageImage CreateValidImageForTeacher(string userId)
        {
            var user = CreateUser(userId);
            var module = CreateModule(includeTeacher: true, user: user);

            var ticket = new FeedbackTicket
            {
                Module = module,
                ModuleId = module.Id,
                Owner = user,
                OwnerId = user.Id,
                Title = "Teacher Ticket",
                Status = FeedbackTicket.TicketStatus.Open
            };

            var message = new FeedbackMessage
            {
                Ticket = ticket,
                TicketId = ticket.TicketId,
                Author = user,
                AuthorId = user.Id,
                Content = "Teacher feedback",
                CreatedAt = DateTime.UtcNow
            };

            return new MessageImage
            {
                Id = 2,
                Message = message,
                MessageId = 2,
                ImageType = "image/png",
                ImageName = "teacher.png",
                ImageData = new byte[] { 4, 5, 6 }
            };
        }

        public static void AssertStudentImageGraph(MessageImage image, string userId)
        {
            Assert.IsNotNull(image);
            Assert.IsNotNull(image.Message);
            Assert.IsNotNull(image.Message.Ticket);
            Assert.IsNotNull(image.Message.Ticket.Module);
            Assert.IsNotNull(image.Message.Ticket.Owner);
            Assert.IsNotNull(image.ImageData);
            Assert.AreEqual("image/jpeg", image.ImageType);
            Assert.AreEqual("image.jpg", image.ImageName);

            Assert.IsNotNull(image.Message.Ticket.Module.StudentModule, "StudentModule is null");
            Assert.IsTrue(image.Message.Ticket.Module.StudentModule.Any(), "StudentModule is empty");
            Assert.IsTrue(
                image.Message.Ticket.Module.StudentModule.Any(sm => sm.UserId == userId),
                $"{userId} not found in StudentModule");

            Assert.IsTrue(image.Message.Ticket.Module.IsUserPartOfModule(userId));
        }
    }
}