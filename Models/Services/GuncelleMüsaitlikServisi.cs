using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectPoliklinik_Servis_Final.Models.Entity;

namespace ProjectPoliklinik_Servis_Final.Models.Services
{
    public class GuncelleMüsaitlikServisi
    {
        public void GuncelleMüsaitlikDurumu()
        {
            using (PoliklinikEntities context = new PoliklinikEntities())
            {
                // Doktorların müsaitlik durumlarını güncelle
                var doktorlar = context.doktor.ToList();
                foreach (var doktor in doktorlar)
                {
                    if (doktor.date_d == 6) // Eğer doktor 5. gündeyse döngüyü sıfırla
                    {
                        doktor.date_d = 1; // Çalışma döngüsünü sıfırla
                        doktor.musaitlik_d = true; // Çalışma günü başlıyor, müsait yap
                    }
                    else if (doktor.date_d == 2) // 2. gün çalışmayı bitirdiğinde dinlenmeye geç
                    {
                        doktor.date_d += 1; // Gün sayısını artır
                        doktor.musaitlik_d = false; // Dinlenmeye geçtiği için müsait değil
                    }
                    else
                    {
                        doktor.date_d += 1; // Gün sayısını artır
                        doktor.musaitlik_d = doktor.date_d <= 2; // 2. gün çalışıyorsa true, diğer durumda false
                    }
                }

                // Hemşirelerin müsaitlik durumlarını güncelle
                var hemsireler = context.hemsire.ToList();
                foreach (var hemsire in hemsireler)
                {
                    if (hemsire.date_h == 6) // Eğer hemşire 5. gündeyse döngüyü sıfırla
                    {
                        hemsire.date_h = 1; // Çalışma döngüsünü sıfırla
                        hemsire.musaitlik_d = true; // Çalışma günü başlıyor, müsait yap
                    }
                    else if (hemsire.date_h == 2) // 2. gün çalışmayı bitirdiğinde dinlenmeye geç
                    {
                        hemsire.date_h += 1; // Gün sayısını artır
                        hemsire.musaitlik_d = false; // Dinlenmeye geçtiği için müsait değil
                    }
                    else
                    {
                        hemsire.date_h += 1; // Gün sayısını artır
                        hemsire.musaitlik_d = hemsire.date_h <= 2; // 2. gün çalışıyorsa true, diğer durumda false
                    }
                }

                // Değişiklikleri kaydet
                context.SaveChanges();
            }
        }
    }
}