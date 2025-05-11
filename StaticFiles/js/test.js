console.log("Скрипт успешно загружен!");

document.addEventListener('DOMContentLoaded', function() {
    const img = document.querySelector('img');
    if (img) {
        img.style.cursor = 'pointer';
        img.addEventListener('click', function() {
            console.log("Клик по изображению зарегистрирован");
        });
    }
});
