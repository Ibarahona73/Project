import express from "express";
import { pool } from '../db.js';

const Router = express.Router();

// POST para crear una nueva nota de texto
Router.post("/crearnotastexto", async (req, res) => {
  const { contenido, reminderDate, tipoderecordatorio, id_usuario } = req.body;

  try {
    const insertNotasTextoQuery = `
      INSERT INTO NotasTexto (Contenido, reminderDate, tipoderecordatorio, CreateDate, UserId)
      VALUES (?, ?, ?, CURRENT_TIMESTAMP, ?)
    `;
    await pool.query(insertNotasTextoQuery, [contenido, reminderDate, tipoderecordatorio, id_usuario]);

    res.status(200).json({ message: "NotasTexto creado exitosamente" });
  } catch (error) {
    console.error('Error al crear NotasTexto:', error.message);
    res.status(500).json({ error: 'Error al crear NotasTexto' });
  }
});

// GET para obtener las notas de texto para un usuario específico
Router.get("/notastexto/:id_usuario", async (req, res) => {
  const { id_usuario } = req.params;

  try {
    const getNotasTextoQuery = `
      SELECT Id, Contenido, reminderDate, CreateDate, tipoderecordatorio
      FROM NotasTexto
      WHERE UserId = ?
    `;
    const notasTextoUsuario = await pool.query(getNotasTextoQuery, [id_usuario]);

    res.status(200).json(notasTextoUsuario);
  } catch (error) {
    console.error('Error al obtener notas de texto para el usuario:', error.message);
    res.status(500).json({ error: 'Error al obtener notas de texto para el usuario' });
  }
});

export default Router;
