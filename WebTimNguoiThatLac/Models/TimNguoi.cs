﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTimNguoiThatLac.Models
{
    [Table("TimNguoi")]
    public class TimNguoi
    {
        
        public  TimNguoi()
        {
            this.AnhTimNguois = new HashSet<AnhTimNguoi>();
            this.BinhLuans = new HashSet<BinhLuan>();
            this.BaoCaoBaiViets = new HashSet<BaoCaoBaiViet>();
        }

        [Key]
        public int Id { get; set; }
        public string? HoTen { get; set; }
        public string? TieuDe { get; set; }
        [DataType(DataType.Html)]
        public string? MoTa { get; set; }
        public string DaciemNhanDang { get; set; } 
        public int? GioiTinh { get; set; }
        public bool active { get; set; } = false;
        public string? TrangThai { get; set; } = "Đang Tìm Kiếm";
        public string? KhuVuc { get; set; }
        public DateTime NgayDang { get; set; } = DateTime.Now;

        public string? MoiQuanHe { get; set; } // Mối quan hệ với người mất tích
        public DateTime? NgayMatTich { get; set; }  // Ngày mất tích
        public  ICollection<AnhTimNguoi>? AnhTimNguois { get; set; }

        public string? IdNguoiDung { get; set; }
        [ForeignKey("IdNguoiDung")]
        public  ApplicationUser? ApplicationUser { get; set; }

        public  ICollection<BinhLuan>? BinhLuans { get; set; }

        public string? DonDangKiTrinhBao { get; set; }

        public ICollection<BaoCaoBaiViet> BaoCaoBaiViets { get; set; }
    }
}
