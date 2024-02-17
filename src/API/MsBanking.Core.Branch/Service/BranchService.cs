using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MsBanking.Common.Dto;
using MsBanking.Core.Branch.Domain;
using System.Text.Json;

namespace MsBanking.Core.Branch.Services
{
    public class BranchService : IBranchService
    {
        private readonly BranchDbContext db;
        private readonly IMapper mapper;
        private readonly IDistributedCache cache;
        private const string cacheKey = "WorkintechMSBanking_Branches";//Cache keyimizi tanımlıyoruz

        public BranchService(BranchDbContext _db, IMapper _mapper, IDistributedCache _cache)
        {
            db = _db;
            mapper = _mapper;
            cache= _cache;
        }

        private async Task<List<BranchResponseDto>> SetCacheBranchList()//Cachede data yoksa bu metodu çağıracaz.Hem veritabanını okuyacak hemde cache yazacak
        {
            var branches = await db.Branches.ToListAsync();//Veritabanından tüm branchları çekiyoruz
            var mapped = mapper.Map<List<BranchResponseDto>>(branches);//Map ediyoruz

            if (mapped.Count > 0)//Eğer veritabanında branch varsa
            { 
                string branchSerialized = JsonSerializer.Serialize(mapped);//Json formatına çeviriyoruz
                var options = new DistributedCacheEntryOptions()//Cache için ayarlarımızı yapıyoruz
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)//60 dakika cache'de kalacak
                };
                await cache.SetStringAsync(cacheKey, branchSerialized, options);//Cache'e yazıyoruz
            }

            return mapped;
        }


        public async Task<List<BranchResponseDto>> GetBranchesAsync()
        {
            var fromCache= await cache.GetStringAsync(cacheKey);//Cache'den datayı çekiyoruz 
            if (!string.IsNullOrEmpty(fromCache))
            {
                List<BranchResponseDto> result= JsonSerializer.Deserialize<List<BranchResponseDto>>(fromCache);//Json formatından listeye çeviriyoruz
                return result;
            }


         List<BranchResponseDto> response= await SetCacheBranchList();//Cachede data yoksa bu metodu çağırıyoruz.Veritabanından almıştık ve cache'e yazmıştıkTekrar veritabanına gitmeye gerek yok

            return response;      
        }

        public async Task<BranchResponseDto> GetBranchByIdAsync(int id)
        {
            var fromCache = await cache.GetStringAsync(cacheKey);//Cache'den datayı çekiyoruz 
            if (!string.IsNullOrEmpty(fromCache))
            {
                List<BranchResponseDto> response = JsonSerializer.Deserialize<List<BranchResponseDto>>(fromCache);//Json formatından listeye çeviriyoruz
                var branchFromCache = response.FirstOrDefault(x => x.Id == id);
                return branchFromCache;
            }


            var branch = await db.Branches.FirstOrDefaultAsync(x => x.Id == id);

            var mapped = mapper.Map<BranchResponseDto>(branch);

            return mapped;
        }

        public async Task<BranchResponseDto> CreateBranchAsync(BranchDto branchDto)
        {
            var branch = mapper.Map<MsBanking.Common.Entity.Branch>(branchDto);

            db.Branches.Add(branch);
            await db.SaveChangesAsync();

            var mapped = mapper.Map<BranchResponseDto>(branch);

            cache.Remove(cacheKey);//Yeni bir branch eklediğimiz için cache'i siliyoruz

            return mapped;
        }

        public async Task<BranchResponseDto> UpdateBranchAsync(int id, BranchDto branchDto)
        {
            var branch = await db.Branches.FirstOrDefaultAsync(x => x.Id == id);

            if (branch == null)
            {
                return null;
            }

            mapper.Map(branchDto, branch);

            db.Branches.Update(branch);
            await db.SaveChangesAsync();

            var mapped = mapper.Map<BranchResponseDto>(branch);

            cache.Remove(cacheKey);//Branch güncellendiği için cache'i siliyoruz

            return mapped;
        }

        public async Task<bool> DeleteBranchAsync(int id)
        {
            var branch = await db.Branches.FirstOrDefaultAsync(x => x.Id == id);

            if (branch == null)
            {
                return false;
            }

            db.Branches.Remove(branch);
            await db.SaveChangesAsync();

            cache.Remove(cacheKey);//Branch silindiği için cache'i siliyoruz.Yeni değerlerle cache'i güncelleyeceğiz

            return true;
        }
    }
}