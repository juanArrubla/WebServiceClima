var btnIngresar = document.getElementById("btnIngresar");
btnIngresar.onclick = function () {
    var usuario = document.getElementById("txtUsuario").value;
    var contra = document.getElementById("txtContra").value;

    if (usuario == "") {
        alert("Ingrese un usuario");
        return;
    }

    if (contra == "") {
        alert("Ingreseuna contraseña");
        return;
    }
        
    $.ajax({
        type: "POST",
        url: "Loguin/validarUsuario?usuario="+usuario+"&contrasena="+contra,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data > 0) {
                document.location.href = "PaginaPrincipal/Index";
            } else {
                alert("El usuario o contraseña incorrecto");
            }
        }
    });
}