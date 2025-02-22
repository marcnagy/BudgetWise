import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import Cookies from "js-cookie";
import PieChartComponent from "../Chart/Chart";
import Expenses from "../Expenses/Expenses";
import "./Dashboard.scss";
import { useState, useEffect } from "react";
import { Expense } from "../../models/expense";
import { getExpenses } from "../../Api/apis";

export default function Dashboard() {
  const navigate = useNavigate();
  const [expenses, setExpenses] = useState<Expense[]>([]);
  const [reload, setReload] = useState(true);

  const handleLogout = () => {
    Cookies.remove("auth_token");
    Cookies.remove("is_logged_in");
    navigate("/");
  };

  const fetchExpenses = async () => {
    try {
      const expensesData = await getExpenses();
      setExpenses(expensesData);
      setReload(false);
    } catch (error) {
      console.error("Failed to fetch expenses:", error);
    }
  };

  useEffect(() => {
    if (reload) fetchExpenses();
  }, [reload]);

  return (
    <div className="dashboardContainer">
      <div className="HeadBar">
        <h1>Budget Wise</h1>
        
        <span className="buttonHolder">
          <Button variant="contained" color="error" onClick={handleLogout}>
          Logout
        </Button>
        </span>
        
      </div>


      {/* Pass expenses & setExpenses to update on new creation */}
      <Expenses setReload={setReload} />
      <PieChartComponent setReload={setReload} expenses={expenses} reload={reload} />

    </div>
  );
}
