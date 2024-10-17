using Microsoft.EntityFrameworkCore;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories
{
    public class BadmintonMatchDetailRepository : IBadmintonMatchDetailRepository
    {
        private readonly NinetyContext _context;

        public BadmintonMatchDetailRepository(NinetyContext context)
        {
            _context = context;
        }

        public async Task<BadmintonMatchDetail> Create(BadmintonMatchDetail badmintonMatchDetail)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.BadmintonMatchDetails.AddAsync(badmintonMatchDetail);
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
            return badmintonMatchDetail;
        }

        public async Task<List<BadmintonMatchDetail>> GetAll()
        {
            return await _context.BadmintonMatchDetails.ToListAsync();
        }

        public async Task<BadmintonMatchDetail> GetById(int id)
        {
            return await _context.BadmintonMatchDetails.Include(c => c.Match).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddMatchDetails(List<BadmintonMatchDetail> matchDetails)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Thêm toàn bộ danh sách chi tiết trận đấu vào DbContext
                    await _context.BadmintonMatchDetails.AddRangeAsync(matchDetails);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    // Commit transaction nếu mọi thứ thành công
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi xảy ra
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }


        public async Task<BadmintonMatchDetail> Update(BadmintonMatchDetail badmintonMatchDetail)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Gán bản ghi cần cập nhật vào trạng thái Modified
                    _context.BadmintonMatchDetails.Update(badmintonMatchDetail);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    // Commit transaction sau khi các thao tác đã thành công
                    await transaction.CommitAsync();

                    // Trả về đối tượng đã được cập nhật
                    return badmintonMatchDetail;
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
