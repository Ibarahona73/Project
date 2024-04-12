using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System;
using static Android.Provider.ContactsContract.CommonDataKinds;

namespace Project.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DashBoardAtras : ContentPage
	{
		public DashBoardAtras()
		{
			InitializeComponent();
			var page = new DashBoardo();
			Navigation.PushAsync(page);
			int id_usuario = Preferences.Get("UserId", defaultValue: 0);
			// Contenido HTML y JavaScript para consumir la API y mostrar los recordatorios con imágenes
			string htmlContent = $@"
          <!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Recordatorios</title>
    <style>
body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 20px;
            padding: 20px;
            max-width: 800px;
            margin: 0 auto;
        }}

        h1 {{
            font-size: 28px;
            color: #333;
            margin-bottom: 20px;
        }}

        h2 {{
            font-size: 24px;
            color: #555;
            margin-top: 30px;
            margin-bottom: 10px;
        }}

        ul {{
            list-style-type: none;
            padding: 0;
            margin: 0;
        }}

        li {{
            background-color: #fff;
            padding: 10px;
            border-radius: 5px;
            margin-bottom: 10px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        }}
        .recordatorio-item {{
            margin-bottom: 20px;
        }}
        .recordatorio-item img {{
            width: 50px;
            height: 50px;
        }}
    .btn-editar {{background - color: #3498db;
            color: #fff;
            border: none;
            padding: 5px 10px;
            border-radius: 3px;
            cursor: pointer;
            margin-right: 5px;
        }}

        .button {{background - color: #e74c3c;
            color: #fff;
            border: none;
            padding: 5px 10px;
            border-radius: 3px;
            cursor: pointer;
        }}
    </style>
</head>
<body>
    <h1>Recordatorios</h1>


    <ul id='notasVozList'></ul>

    <ul id='imagenesList'></ul>

    <ul id='notasTextoList'></ul>

    <script>

function editarNota(idNota, contenidoNota) {{
    const nuevoContenido = prompt('Editar Nota de Texto', contenidoNota);
    if (nuevoContenido !== null) {{
        fetch(`http://3.129.71.4:3000/editarnotastexto/${{idNota}}`, {{
            method: 'PUT',
            headers: {{
                'Content-Type': 'application/json'
            }},
            body: JSON.stringify({{ contenido: nuevoContenido }})
        }})
        .then(response => response.json())
       .then(data => {{
    console.log('Nota de texto editada exitosamente:', data);
    // Mostrar alerta de éxito
    alert('Nota de texto editada exitosamente');
    // Limpiar la lista antes de agregar las nuevas notas
    notasTextoList.innerHTML = '';
    // Actualizar la UI
    mostrarNotasTexto();
}})


       
    }}
}}


function eliminarNota(idNota) {{
    const mensajeConfirmacion = '¿Estás seguro de que deseas eliminar esta nota?';
    if (confirm(mensajeConfirmacion)) {{
        fetch(`http://3.129.71.4:3000/eliminarnotastexto/${{idNota}}`, {{
            method: 'DELETE'
        }})
        .then(response => response.json())
       .then(data => {{
    console.log('Nota de texto eliminada exitosamente:', data);
    // Mostrar alerta de éxito
    alert('Nota de texto eliminada exitosamente');
    // Limpiar la lista antes de agregar las nuevas notas
    notasTextoList.innerHTML = '';
    // Actualizar la UI
    mostrarNotasTexto();
}})


       
    }}
}} 

        document.addEventListener('DOMContentLoaded', function() {{
            const notasVozList = document.getElementById('notasVozList');
            const imagenesList = document.getElementById('imagenesList');
            const notasTextoList = document.getElementById('notasTextoList');

            // Función para mostrar las Notas de Voz
            function mostrarNotasVoz() {{
                fetch('http://3.129.71.4:3000/notasvoz/{id_usuario}')
                    .then(response => response.json())
                    .then(data => {{
                        const notasVoz = Array.isArray(data[0]) ? data[0] : data;

                        notasVoz.forEach(nota => {{
                            const listItem = document.createElement('li');
                            listItem.innerHTML = `
                                <strong>Fecha de Recordatorio:</strong> ${{new Date(nota.reminderDate).toLocaleString()}}<br>
                                <button onclick=""reproducirAudio('${{nota.RutaArchivo}}')"">Reproducir Audio</button><br>
                                <strong>estado:</strong> <input type=""checkbox"" disabled ${{nota.estado === '1' ? 'checked' : ''}}><br>
                                 
                            `;
                            notasVozList.appendChild(listItem);
                        }});
                    }})
                    .catch(error => {{
                        console.error('Error al obtener las notas de voz:', error);
                        notasVozList.innerHTML = '<li>Error al cargar las notas de voz.</li>';
                    }});
            }}

            // Función para mostrar los Recordatorios con Imágenes
            function mostrarRecordatoriosConImagenes() {{
                fetch('http://3.129.71.4:3000/recordatorios-con-imagenes/{id_usuario}')
                    .then(response => response.json())
                    .then(data => {{
                        const recordatoriosImagenes = Array.isArray(data[0]) ? data[0] : data;

                        recordatoriosImagenes.forEach(recordatorio => {{
                            const listItem = document.createElement('li');
                            listItem.classList.add('recordatorio-item');

                            const imagen = new Image();
                            imagen.src = 'data:image/jpeg;base64,' + recordatorio.RutaArchivo.data;
                            imagen.style.width = '30px';
                            imagen.style.height = '30px';
                            listItem.appendChild(imagen);

                            listItem.innerHTML += `
                                <br><strong>Descripción:</strong> ${{recordatorio.description}}<br>
                                <strong>Fecha de Recordatorio:</strong> ${{new Date(recordatorio.reminderDate).toLocaleString()}}<br>
                               <strong>estado:</strong> <input type=""checkbox"" disabled ${{recordatorio.estado === '1' ? 'checked' : ''}}><br>
                           
                            `;
                            imagenesList.appendChild(listItem);
                        }});
                    }})
                    .catch(error => {{
                        console.error('Error al obtener los recordatorios con imágenes:', error);
                        imagenesList.innerHTML = '<li>Error al cargar los recordatorios con imágenes.</li>';
                    }});
            }}

            // Función para mostrar las Notas de Texto
            function mostrarNotasTexto() {{
                fetch('http://3.129.71.4:3000/notastexto/{id_usuario}')
                    .then(response => response.json())
                    .then(data => {{
                        const notasTexto = Array.isArray(data[0]) ? data[0] : data;

                        notasTexto.forEach(nota => {{
                            const listItem = document.createElement('li');
                            listItem.innerHTML = `
                                <strong>Contenido:</strong> ${{nota.Contenido}}<br>
                                <strong>Fecha de Recordatorio:</strong> ${{new Date(nota.reminderDate).toLocaleString()}}<br>
                                <button onclick=""editarNota('${{nota.Id}}', '${{nota.Contenido}}')"">Editar</button>
                                  <button onclick=""eliminarNota('${{nota.Id}}')"">Eliminar</button><br>
                                    <strong>estado:</strong> <input type=""checkbox"" disabled ${{nota.estado === '1' ? 'checked' : ''}}><br>

                            `;

                            notasTextoList.appendChild(listItem);
                        }});
                    }})
                    .catch(error => {{
                        console.error('Error al obtener las notas de texto:', error);
                        notasTextoList.innerHTML = '<li>Error al cargar las notas de texto.</li>';
                    }});
            }}


            // Función para reproducir el audio
            function reproducirAudio(rutaArchivo) {{
                const audio = new Audio('data:audio/mp3;base64,' + rutaArchivo);
                audio.play();
            }}

           // Llamamos a todas las funciones para mostrar los recordatorios
            mostrarNotasVoz();
            mostrarRecordatoriosConImagenes();
            mostrarNotasTexto();
        }});


    </script>
</body>
</html>


            ";

			MyWebView.Source = new HtmlWebViewSource
			{
				Html = htmlContent
			};
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			ActualizarDatosWebView();
		}

		private void ActualizarDatosWebView()
		{
			// Lógica para actualizar los datos en el WebView
			MyWebView.Eval("mostrarNotasVoz();");
			MyWebView.Eval("mostrarRecordatoriosConImagenes();");
			MyWebView.Eval("mostrarNotasTexto();");
		}
	}
}
