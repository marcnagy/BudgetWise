import { useState } from "react";
import { PieChart, Pie, Cell, Tooltip, Legend } from "recharts";
import { Expense } from "../../models/expense";
import ExpenseModal from "../Modal/ExpenseModal"; // Import the modal
import "./Chart.scss";
const COLORS = ["#0088FE", "#00C49F", "#FFBB28", "#FF8042", "#8884d8"];

export default function PieChartComponent({setReload, expenses }: {setReload:Function, expenses: Expense[]; reload: Boolean }) {
  const [selectedExpense, setSelectedExpense] = useState<Expense | null>(null);

  const chartData = expenses.map(expense => ({
    name: expense.name,
    value: expense.amount,
    id: expense.id, 
  }));

  const handleSliceClick = (_: any, index: number) => {
    const clickedExpense = expenses[index]; // Get full expense data
    setSelectedExpense(clickedExpense); // Open modal with clicked expense
  };

  return (
    <>
    {expenses.length > 0 ? <PieChart width={400} height={400}>
        <Pie
          data={chartData}
          cx="50%"
          cy="50%"
          outerRadius={120}
          fill="#8884d8"
          dataKey="value"
          label
          onClick={handleSliceClick} // Open modal on slice click
        >
          {chartData.map((_, index) => (
            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
          ))}
        </Pie>
        <Tooltip />
        <Legend />
      </PieChart> : 
      <div className="emptyContainer">
          <img src="assets/image/cash.png" alt="" />
          <h2>No Expenses Yet , add some</h2>

      </div>
      }
      

      {/* Expense Modal */}
      {selectedExpense && (
        <ExpenseModal
          expense={selectedExpense}
          onClose={() => setSelectedExpense(null)}
          onUpdate={() => setReload(true)} // Pass reload function if needed
          onDelete={() => setReload(true)} // Pass reload function if needed
        />
      )}
    </>
  );
}
