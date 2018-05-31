function GetImageData() {
    $.ajax({
        type: "GET",
        url: '/Image/GetImageData/',
        data: { imageFilePath: "C:/Users/Bas/Desktop/6d0369ad-c6e7-4901-ba26-62781f26801b.jpg" },
        success: function (result) {
            document.getElementById('piellemuis').innerHTML = result;
        },
        error: function (req, status, error) {
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });
}