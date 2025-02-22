import { useState } from "react";
import { TextField, Button, Box } from "@mui/material";
import { createExpense } from "../../Api/apis";
import "./Expenses.scss";
interface props {
    setReload : Function
}
export default function ExpenseForm({ setReload }: props) {
  const [expense, setExpense] = useState({
    name: "",
    amount: 0,
    date: new Date().toISOString().split("T")[0],
    description: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setExpense({
      ...expense,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await createExpense(expense);
      alert("Expense created successfully!");
      setReload(true); // 🔄 Refresh expenses after creating one
    } catch (error) {
      alert("Failed to create expense");
    }
  };

  return (
    <>
    <div className="header">
      <h1>Expenses</h1>
      <Button onClick={handleSubmit} variant="contained">Create Expense</Button>
    </div>
    <div className="Form">
      <TextField label="Name" name="name" value={expense.name} onChange={handleChange} required />
      <TextField label="Amount" name="amount" type="number" value={expense.amount} onChange={handleChange} required />
      <TextField label="Date" name="date" type="date" value={expense.date} onChange={handleChange} required />
      <TextField label="Description" name="description" value={expense.description} onChange={handleChange}  rows={3} />
    </div>

    </>
    
    
    
  );
}
