using StoreManagement.DTOs.Request;
using StoreManagement.DTOs.Response;
using StoreManagement.Models;

namespace StoreManagement.Mapper
{
    public class UserMapper : IMapper<User, UserResponse>
    {
        public UserResponse ToDto(User entity)
        {
            return new UserResponse
            {
                UserId = entity.UserId,
                Username = entity.Username,
                FullName = entity.FullName,
                Role = entity.Role,
                CreatedAt = entity.CreatedAt
            };
        }

        public User ToModel(UserResponse dto)
        {
            return new User
            {
                UserId = dto.UserId,
                Username = dto.Username,
                FullName = dto.FullName,
                Role = dto.Role,
                CreatedAt = dto.CreatedAt
            };
        }

        public User ToModel(UserCreateRequest dto)
        {
            return new User
            {
                Username = dto.Username.Trim(),
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName?.Trim(),
                Role = dto.Role.Trim().ToLowerInvariant(),
                CreatedAt = DateTime.UtcNow
            };
        }

        public IEnumerable<UserResponse> ToDtoList(IEnumerable<User> entities)
        {
            return entities.Select(entity => ToDto(entity)).ToList();
        }

        public IEnumerable<User> ToModelList(IEnumerable<UserResponse> dtos)
        {
            return dtos.Select(dto => ToModel(dto)).ToList();
        }

        public void MapToExistingModel(UserResponse dto, User entity)
        {
            entity.Username = dto.Username;
            entity.FullName = dto.FullName;
            entity.Role = dto.Role;
        }

        public void MapToExistingModel(UserUpdateRequest dto, User entity)
        {
            if (dto.Role != null)
            {
                entity.Role = dto.Role.Trim().ToLowerInvariant();
            }

            if (dto.FullName != null)
            {
                entity.FullName = dto.FullName.Trim();
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                entity.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }
        }
    }
}
