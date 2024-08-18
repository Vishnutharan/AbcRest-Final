//using AbcRest_Final.Database_Context;

//namespace AbcRest_Final.Service
//{
//    public class UserService : IUserService
//    {
//        private readonly ApplicationDbContext _context;

//        public UserService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<ServiceResult> RegisterUser(UserDto userDto)
//        {
//            var user = new User
//            {
//                Username = userDto.Username,
//                PasswordHash = userDto.PasswordHash, // Consider hashing this if not already hashed
//                Email = userDto.Email
//            };

//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            return new ServiceResult { IsSuccess = true, Message = "User registered successfully." };
//        }
//    }
//}
