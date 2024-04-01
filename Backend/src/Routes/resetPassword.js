import express from "express";
import { Resend } from "resend";
import { pool } from '../db.js'; // Importa el objeto pool de tu archivo db.js

const Router = express.Router();
const resend = new Resend('re_YAUmMuHn_HpYn2P3Vr1Zysm7TbYiU6GCs');

Router.post("/", async (req, res) => {
  const { email } = req.body;
  if (!email) {
    return res.status(400).json({ error: "Se requiere un correo electrónico" });
  }

  try {
    // Verificar si el correo existe en la base de datos
    const [existingUser] = await pool.query('SELECT * FROM Usuarios WHERE email = ?', [email]);
    
    if (existingUser.length === 0) {
      return res.status(404).json({ error: "Correo electrónico no encontrado en la base de datos" });
    }

    let codigoRecuperacion = Math.floor(100000 + Math.random() * 900000);

    // Verificar si el usuario ya tiene un código de recuperación
    if (existingUser[0].codigo_recuperacion) {
      // Si ya tiene un código, actualizarlo en lugar de generar uno nuevo
      codigoRecuperacion = existingUser[0].codigo_recuperacion;
    } else {
      // Guardar el código de recuperación en la base de datos si no tiene uno
      const insertQuery = 'UPDATE Usuarios SET codigo_recuperacion = ? WHERE email = ?';
      await pool.query(insertQuery, [codigoRecuperacion, email]);
    }

    // Enviar el código de recuperación por correo electrónico
    const { data, error } = await resend.emails.send({
      from: "Acme <onboarding@resend.dev>",
      to: [email],
      subject: "Código de recuperación de contraseña",
      html: `<strong>Su código de recuperación es:</strong> ${codigoRecuperacion}`,
    });

    if (error) {
      return res.status(400).json({ error });
    }

    res.status(200).json({ message: "Código de recuperación enviado por correo electrónico" });

  } catch (dbError) {
    console.error('Error al interactuar con la base de datos:', dbError.message);
    res.status(500).json({ error: 'Error al generar el código de recuperación' });
  }
});


export default Router;
