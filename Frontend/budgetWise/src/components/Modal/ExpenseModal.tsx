import { useState } from "react";
import { Modal, Box, TextField, Button, Typography } from "@mui/material";
import { Expense } from "../../models/expense";
import { updateExpense, deleteExpense } from "../../Api/apis"; // API calls

interface ExpenseModalProps {
  expense: Expense | null;
  onClose: () => void;
  onUpdate: () => void;
  onDelete: () => void;
}

export default function ExpenseModal({ expense, onClose, onUpdate, onDelete }: ExpenseModalProps) {
  const [editedExpense, setEditedExpense] = useState<Expense | null>(expense);

  if (!expense) return null; // If no expense, don't render modal

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEditedExpense({
      ...editedExpense!,
      [e.target.name]: e.target.value,
    });
  };

  const handleSave = async () => {
    if (editedExpense) {
      try {
        await updateExpense(editedExpense.id, editedExpense);
        onUpdate(); // Refresh data
        onClose();
      } catch (error) {
        alert("Failed to update expense.");
      }
    }
  };

  const handleDelete = async () => {
    if (expense) {
      try {
        await deleteExpense(expense.id);
        onDelete(); // Refresh data
        onClose();
      } catch (error) {
        alert("Failed to delete expense.");
      }
    }
  };

  return (
    <Modal open={!!expense} onClose={onClose}>
      <Box sx={{
        position: "absolute", top: "50%", left: "50%", transform: "translate(-50%, -50%)",
        width: 400, bgcolor: "background.paper", boxShadow: 24, p: 3, borderRadius: 2
      }}>
        <Typography variant="h6">Edit Expense</Typography>
        
        <TextField
          label="Name"
          name="name"
          value={editedExpense?.name || ""}
          onChange={handleChange}
          fullWidth
          sx={{ mt: 2 }}
        />
        
        <TextField
          label="Amount"
          name="amount"
          type="number"
          value={editedExpense?.amount || ""}
          onChange={handleChange}
          fullWidth
          sx={{ mt: 2 }}
        />

        <Box sx={{ display: "flex", justifyContent: "space-between", mt: 3 }}>
          <Button variant="contained" color="primary" onClick={handleSave}>Save</Button>
          <Button variant="contained" color="error" onClick={handleDelete}>Delete</Button>
          <Button variant="outlined" onClick={onClose}>Cancel</Button>
        </Box>
      </Box>
    </Modal>
  );
}
