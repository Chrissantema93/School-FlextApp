function GetImageData() {
    $.ajax({
        type: "POST",
        url: '/Image/aquireFiles/',
        data: { files: "C:/Users/Bas/Desktop/alles.jpg" },
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
