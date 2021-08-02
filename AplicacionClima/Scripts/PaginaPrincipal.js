$("#datepickerInicio").datepicker({
    dateFormat: "dd/mm/yy",
    changeMonth: true,
    changeYear: true
});

listar();

function listar() {
    $.get("listarClimas", function (data) {
        crearListado(["Id", "Clima", "Ciudad", "Grados", "Dia"], data);
    })
}

$.get("listarMaestro?tabla=paises&filtro= ", function (data) {
    llenarCombo(data, document.getElementById("cmbPais"), true);
})

var pais = document.getElementById("cmbPais");
var depto = document.getElementById("cmbDepto");
var ciudad = document.getElementById("cmbDepto");
var btnBuscar = document.getElementById("btnBuscar");

pais.onchange = function () {
    $.get("listarMaestro/?tabla=departamentos&filtro=where pais=" + pais.value, function (data) {
        llenarCombo(data, document.getElementById("cmbDepto"), true);
        });
}

depto.onchange = function () {
    $.get("listarMaestro/?tabla=ciudades&filtro=where departamento=" + depto.value, function (data) {
        llenarCombo(data, document.getElementById("cmbCiudad"), true);
    });
}

//Este es para llenar el ComboBox del Modal
$.get("listarMaestro?tabla=ciudades", function (data) {
    llenarCombo(data, document.getElementById("cmbCiudadPopup"), true);
})

btnBuscar.onclick = function () {
    $.get("consultarClima?ciudad=" + ciudad.value, function (data) {
            crearListado(["Id", "Clima", "Ciudad", "Grados", "Dia"], data);
        })
}


function llenarCombo(data, control, primerElemento) {
    var contenido = "";
    if (primerElemento) {
        contenido += "<option value=''>--Seleccione--</option>"
    }

    for (var i = 0; i < data.length; i++) {
        contenido += "<option value='" + data[i].id + "'>";
        contenido += data[i].nombre;
        contenido += "</option>";
    }

    control.innerHTML = contenido;
}


crearListado = (titulos, data) => {
    var keys = Object.keys(data[0])
    keys.shift();
    var contenido = "<table id='tablas' class='table'>"
    contenido += "<thead>"
    contenido += "<tr>"
    for (var titulo of titulos) {
        contenido += "<td>" + titulo + "</td>"
    }
    contenido += "<td>Acciones</td>"
    contenido += "</tr>"
    contenido += "</thead>"
    contenido += "<tbody>"
    for (var i = 0; i < data.length; i++) {
        contenido += "<tr>"
        for (key of keys) {
            contenido += "<td>" + data[i][key] + "</td>"
        }
        contenido += "<td>"
        contenido += "<button class='btn btn-primary' Onclick='abrirModal(" + data[i][keys[0]] + ")' data-toggle='modal' data-target='#myModal'>Editar</button> "
        contenido += "<button class='btn btn-danger' Onclick='eliminar(" + data[i][keys[0]] + ")'>Eliminar</button>"
        contenido += "</td>"
        contenido += "</tr>"
    }

    contenido += "</tbody>"
    contenido += "</table>"

    document.getElementById("tabla").innerHTML = contenido;
}

limpiarDatos = () => {
    const controles = document.getElementsByClassName('limpiar')
    for (var i = 0; i < controles.length; i++) {
        controles[i].value = ""
    }
}

camposObligatorios = () => {
    var exito = true
    const controles = document.getElementsByClassName('obligatorio')
    for (var i = 0; i < controles.length; i++) {
        if (controles[i].value == "") {
            exito = false
            controles[i].parentNode.classList.add('error') 
        }
        else {
            exito = true
            controles[i].parentNode.classList.remove('error')
        }
    }

    return exito
}

function abrirModal(id) {
    var controles = document.getElementsByClassName("obligatorio");
    for (var i = 0; i < controles.length; i++) {
        controles[i].parentNode.classList.remove("error");
    }

    if (id == 0) {
        limpiarDatos();
    } else {
        $.get("recuperarClima/?id=" + id, function (data) {
            document.getElementById("txtId").value = data[0].id;
            document.getElementById("txtNombre").value = data[0].nombre;
            document.getElementById("cmbCiudadPopup").value = data[0].ciudad;
            document.getElementById("txtGrados").value = data[0].grados;
            document.getElementById("datepickerInicio").value = data[0].fechaHora;
        });
    }
}

function agregar() {
    if (camposObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtId").value;
        var nombre = document.getElementById("txtNombre").value;
        var ciudad = document.getElementById("cmbCiudadPopup").value;
        var grados = document.getElementById("txtGrados").value;
        var fechainicio = document.getElementById("datepickerInicio").value;

        frm.append("id", id);
        frm.append("nombre", nombre);
        frm.append("ciudad", ciudad);
        frm.append("grados", grados);
        frm.append("fechahora", fechainicio);
        
        if (confirm("Esta seguro que desea grabar?") == 1) {
            $.ajax({
                type: "POST",
                url: "guardarClima",
                data: frm,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data == 1) {
                        alert("Se ejecuto correctamente")
                        listar();
                        document.getElementById("btnCancelar").click();
                    } else {
                        alert("Ocurrio un error");
                    }

                }
            });
        }
    }
}

function eliminar(id) {
    if (confirm("Esta seguro que desea eliminar el registro") == 1) {
        $.get("eliminarClima?id=" + id, function (data) {
            if (data == 0) {
                alert("Ocurrio un error");
            } else {
                alert("Se elimino el registro correctamente");
                listar();
            }
        });
    }

}
