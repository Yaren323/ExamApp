using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamApp.Service.Interfaces;
using ExamApp.Core.Entities;

public interface IUserService
{
    Task<User> AuthenticateAsync(string username, string password);
    Task<User> GetByIdAsync(int id);
    Task<User> CreateAsync(User user, string password);
    Task<bool> UserExistsAsync(string username);
}