import { useEffect, useState } from "react";
import { TextField, Button, IconButton, InputAdornment } from "@mui/material";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { motion } from "framer-motion";
import { FaUser } from "react-icons/fa";
import { useNavigate } from "react-router-dom";
import "./Login.scss";
import Cookies from "js-cookie";
import { loginUser } from "../../Api/apis";

export default function Login() {
  const [login, setLogin] = useState({ email: "", password: "" });
  const [errors, setErrors] = useState({ email: "", password: "" });
  const [showPassword, setShowPassword] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    if (Cookies.get("auth_token") && Cookies.get("is_logged_in") === "true") {
      navigate("/dashboard");
    }
  }, [navigate]);

  const validateInput = () => {
    let emailError = "";
    let passwordError = "";

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;

    if (!emailRegex.test(login.email)) {
      emailError = "Invalid email format";
    }

    if (!passwordRegex.test(login.password)) {
      passwordError = "Password must be at least 6 characters and include letters & numbers";
    }

    setErrors({ email: emailError, password: passwordError });

    return !emailError && !passwordError;
  };

  const handleLogin = async () => {
    if (!validateInput()) return;

    try {
      const response = await loginUser(login.email, login.password);
      console.log(response.token);
     
      
      if (response) {
        Cookies.set("auth_token", response.token); // no expiry date for sso 
        Cookies.set("is_logged_in", "true");
        navigate("/dashboard");
      } else {
        setErrors({ email: "", password: "Invalid email or password" });
      }
    } catch (error) {
      console.error("Login failed:", error);
    }
  };

  return (
    <motion.div
      className="login-container"
      initial={{ opacity: 0, y: -50 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
    >
      <div className="login-div">
        <FaUser className="user-icon" />
        <h1 className="login-title">Sign In</h1>
        <TextField
          label="Email"
          variant="outlined"
          fullWidth
          margin="normal"
          value={login.email}
          onChange={(e) => setLogin({ ...login, email: e.target.value })}
          type="email"
          error={!!errors.email}
          helperText={errors.email}
        />

        <TextField
          label="Password"
          variant="outlined"
          fullWidth
          margin="normal"
          type={showPassword ? "text" : "password"}
          value={login.password}
          onChange={(e) => setLogin({ ...login, password: e.target.value })}
          error={!!errors.password}
          helperText={errors.password}
          InputProps={{
            endAdornment: (
              <InputAdornment position="end">
                <IconButton onClick={() => setShowPassword(!showPassword)}>
                  {showPassword ? <VisibilityOff /> : <Visibility />}
                </IconButton>
              </InputAdornment>
            ),
          }}
        />

        {/* Sign In Button */}
        <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }}>
          <Button onClick={handleLogin} variant="contained" color="primary" fullWidth>
            Sign In
          </Button>
        </motion.div>

        {/* Sign Up Button */}
        <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }} style={{ marginTop: "10px" }}>
          <Button onClick={() => navigate("/signup")} variant="outlined" color="secondary" fullWidth>
            Create an Account
          </Button>
        </motion.div>
      </div>
    </motion.div>
  );
}
