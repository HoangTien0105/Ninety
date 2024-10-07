using Microsoft.EntityFrameworkCore;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly NinetyContext _context;

        public MatchRepository(NinetyContext context)
        {
            _context = context;
        }

        public async Task<Match> Create(Match match)
        {
            // Sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Matchs.AddAsync(match);
                    await _context.SaveChangesAsync();

                    // Commit transaction sau khi các thao tác đã thành công
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return match;
        }

        public async Task<List<Match>> GetAll()
        {
            return await _context.Matchs.ToListAsync();
        }

        public async Task<Match> GetById(int id)
        {
            return await _context.Matchs.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Match> Update(Match match)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Gán bản ghi cần cập nhật vào trạng thái Modified
                    _context.Matchs.Update(match);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    // Commit transaction sau khi các thao tác đã thành công
                    await transaction.CommitAsync();

                    // Trả về đối tượng đã được cập nhật
                    return match;
                }
                catch (Exception)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
