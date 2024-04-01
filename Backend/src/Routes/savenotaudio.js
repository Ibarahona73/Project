import express from "express";
import { pool } from '../db.js';

const Router = express.Router();

// POST para crear una nueva nota de voz
Router.post("/crearnotasvoz", async (req, res) => {
  const { rutaArchivo, reminderDate, tiporecordatorio, id_usuario } = req.body;

  try {
    const insertNotasVozQuery = `
      INSERT INTO NotasVoz (RutaArchivo, reminderDate, tiporecordatorio, CreateDate, UserId)
      VALUES (?, ?, ?, CURRENT_TIMESTAMP, ?)
    `;
    await pool.query(insertNotasVozQuery, [rutaArchivo, reminderDate, tiporecordatorio, id_usuario]);

    res.status(200).json({ message: "NotasVoz creada exitosamente" });
  } catch (error) {
    console.error('Error al crear NotasVoz:', error.message);
    res.status(500).json({ error: 'Error al crear NotasVoz' });
  }
});

// GET para obtener las notas de voz para un usuario específico
Router.get("/notasvoz/:id_usuario", async (req, res) => {
  const { id_usuario } = req.params;

  try {
    const getNotasVozQuery = `
      SELECT Id, RutaArchivo, reminderDate, CreateDate, tiporecordatorio
      FROM NotasVoz
      WHERE UserId = ?
    `;
    const notasVozUsuario = await pool.query(getNotasVozQuery, [id_usuario]);

    res.status(200).json(notasVozUsuario);
  } catch (error) {
    console.error('Error al obtener notas de voz para el usuario:', error.message);
    res.status(500).json({ error: 'Error al obtener notas de voz para el usuario' });
  }
});

export default Router;
