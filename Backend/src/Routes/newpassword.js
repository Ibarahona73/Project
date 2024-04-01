import express from "express";
import { pool } from '../db.js'; // Importa el objeto pool de tu archivo db.js

const Router = express.Router();

Router.put("/", async (req, res) => {
  console.log(req.body); // Verifica los datos recibidos en el cuerpo de la solicitud

  const { Email, codigo_recuperacion, nuevaContrasena } = req.body;
 // Ajuste en las claves de destructuración

  // Verificar si se proporcionaron el correo electrónico, el código de recuperación y la nueva contraseña
  if (!Email || !codigo_recuperacion || !nuevaContrasena) {
    return res.status(400).json({ error: "Se requiere el correo electrónico, el código de recuperación y la nueva contraseña" });
  }

  // Verificar si el código de recuperación es válido para el correo electrónico dado
  try {
    const [existingUser] = await pool.query('SELECT * FROM Usuarios WHERE email = ? AND codigo_recuperacion = ?', [Email, codigo_recuperacion]);
    
    if (existingUser.length === 0) {
      return res.status(404).json({ error: "Código de recuperación inválido" });
    }

    // Actualizar la contraseña en la base de datos
    const updateQuery = 'UPDATE Usuarios SET Password = ? WHERE email = ?';
    await pool.query(updateQuery, [nuevaContrasena, Email]);

    res.status(200).json({ message: "Contraseña actualizada exitosamente" });

  } catch (dbError) {
    console.error('Error al interactuar con la base de datos:', dbError.message);
    res.status(500).json({ error: 'Error al actualizar la contraseña' });
  }
});

export default Router;
