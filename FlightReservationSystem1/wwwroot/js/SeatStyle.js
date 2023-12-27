function IsSeatAvailable(seatName) {
    // Koltuk müsait kontrolü yapılabilir, örneğin bir JavaScript fonksiyonu ile sunucuya AJAX isteği gönderilebilir.
    return true; // Örnek olarak her koltuk müsait olarak kabul ediliyor.
}

function GetSeatColor(seatName) {
    // Koltuğun rengini belirleme fonksiyonu
    // Örneğin, yeşil boş, sarı seçili, kırmızı rezerve edilmiş gibi durumları kontrol edebilirsiniz.
    if (!IsSeatAvailable(seatName)) {
        return "red"; // Eğer rezerve edilmişse kırmızı
    } else {
        return "green"; // Aksi durumda yeşil
    }
}
