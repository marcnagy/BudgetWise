import { useState } from "react";
import { TextField, Button, IconButton, InputAdornment } from "@mui/material";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { motion } from "framer-motion";
import { FaUserPlus } from "react-icons/fa";
import { Link, useNavigate } from "react-router-dom";
import "./Signup.scss";
import { registerUser } from "../../Api/apis";

export default function Signup() {
  const navigate = useNavigate();

  const [signup, setSignup] = useState({
    name: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const [errors, setErrors] = useState({
    name: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const [showPassword, setShowPassword] = useState(false);

  const validateInput = () => {
    let nameError = "";
    let emailError = "";
    let passwordError = "";
    let confirmPasswordError = "";

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;

    if (!signup.name.trim()) {
      nameError = "Name is required";
    }

    if (!emailRegex.test(signup.email)) {
      emailError = "Invalid email format";
    }

    if (!passwordRegex.test(signup.password)) {
      passwordError = "Password must be at least 6 characters and include letters & numbers";
    }

    if (signup.password !== signup.confirmPassword) {
      confirmPasswordError = "Passwords do not match";
    }

    setErrors({ name: nameError, email: emailError, password: passwordError, confirmPassword: confirmPasswordError });

    return !nameError && !emailError && !passwordError && !confirmPasswordError;
  };

  const handleSignup = async () => {
    if (!validateInput()) return;
  
    try {
      await registerUser(signup.name, signup.email, signup.password);
      alert("Signup successful! Redirecting to login...");
      navigate("/");
    } catch (error) {
      alert("Something went wrong. Please try again.");
    }
  };

  return (
    <motion.div
      className="signup-container"
      initial={{ opacity: 0, y: -50 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
    >
      <div className="signup-div">
        <FaUserPlus className="user-icon" />
        <h1 className="signup-title">Sign Up</h1>

        <TextField
          label="Full Name"
          variant="outlined"
          fullWidth
          margin="normal"
          value={signup.name}
          onChange={(e) => setSignup({ ...signup, name: e.target.value })}
          error={!!errors.name}
          helperText={errors.name}
        />

        <TextField
          label="Email"
          variant="outlined"
          fullWidth
          margin="normal"
          value={signup.email}
          onChange={(e) => setSignup({ ...signup, email: e.target.value })}
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
          value={signup.password}
          onChange={(e) => setSignup({ ...signup, password: e.target.value })}
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

        <TextField
          label="Confirm Password"
          variant="outlined"
          fullWidth
          margin="normal"
          type={showPassword ? "text" : "password"}
          value={signup.confirmPassword}
          onChange={(e) => setSignup({ ...signup, confirmPassword: e.target.value })}
          error={!!errors.confirmPassword}
          helperText={errors.confirmPassword}
        />

        <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }}>
          <Button onClick={handleSignup} variant="contained" color="primary" fullWidth>
            Sign Up
          </Button>
        </motion.div>

        <p className="switch-auth">
          Already have an account? <Link to="/login">Sign In</Link>
        </p>
      </div>
    </motion.div>
  );
}
