window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    new QRCode(document.getElementById("qrCode"),
        {
            text: uri,
            width: 150,
            height: 150
        });
});

@section Scripts {
    @await Html.PartialAsync("_ValidationScriptsPartial")

       <script type = "text/javascript" src = "~/lib/qrcode.js" ></script >
        <script type="text/javascript" src="~/js/qr.js" ></script>
}