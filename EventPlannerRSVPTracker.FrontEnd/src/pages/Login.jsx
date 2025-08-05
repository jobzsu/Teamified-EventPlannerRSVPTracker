import React, { useState } from 'react';
import { Box, TextField, Button, Typography, Container, Paper, CircularProgress } from '@mui/material';
import AppHttpClient from '../utils/AppHttpClient';
import ToastifyHelper from '../utils/ToastifyHelper';
import { useNavigate } from "react-router-dom";

const LoadingSpinner = () => {
  return (
    <Box 
      sx={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        height: '200px'
      }}
    >
      <CircularProgress />
    </Box>
  );
};

const Login = () => {
  const [username, setUsername] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const navigate = useNavigate();

  const handleUsernameChange = (event) => {
    setUsername(event.target.value);
  };

  const handleLogin = () => {
    setIsLoading(true);

    console.log(`Attempting to log in with username: ${username}`);

    let isSuccess = false;
    let errMssg = '';
    
    AppHttpClient.post('/users/login', username )
      .then((response) => {
        isSuccess = response.data.isSuccess;
        if(!isSuccess)
            errMssg = response.data.message;
        else {
          sessionStorage.setItem('userId', response.data.data.toString());
          sessionStorage.setItem('username', username);
        }
      })
      .catch((error) => {
        errMssg = error.response.data.message;
      })
      .finally(() => {
        if(isSuccess) {
          ToastifyHelper.AlertSuccessNotif('Login successful!', 'loginSuccess');
          setTimeout(() => {
            navigate('/my-events');
          }, 2000);
        }
        else {
          setIsLoading(false);
          ToastifyHelper.AlertErrorNotif(errMssg, 'loginError');
        }
      });
  };

  return (
    <Container
      component="main"
      maxWidth="xs"
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        minHeight: '100vh',
        backgroundColor: '#f3f4f6', // Light gray background
        p: 2, // Padding
      }}
    >
      <Paper elevation={12} sx={{ p: 4, width: '100%', borderRadius: 3, transition: 'transform 0.3s', '&:hover': { transform: 'scale(1.05)' } }}>
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
          }}
        >
          {/* Main heading for the application */}
          <Typography component="h1" variant="h3" sx={{ mb: 4, fontWeight: 'bold', color: '#1f2937' }}>
            Event Planner
          </Typography>

          {/* Conditional rendering: show loading spinner or the form */}
          {isLoading ? (
            <LoadingSpinner />
          ) : (
            <>
              {/* Input field for username */}
              <TextField
                margin="normal"
                required
                fullWidth
                id="username"
                label="Username"
                name="username"
                autoComplete="username"
                autoFocus
                value={username}
                onChange={handleUsernameChange}
                variant="outlined"
                sx={{ mb: 2 }}
              />

              {/* Login button */}
              <Button
                type="submit"
                fullWidth
                variant="contained"
                onClick={handleLogin}
                // The button is disabled while the login is in progress.
                disabled={isLoading}
                sx={{
                  mt: 1,
                  mb: 2,
                  py: 1.5,
                  fontWeight: 'bold',
                  borderRadius: 2,
                  backgroundColor: '#4f46e5',
                  '&:hover': {
                    backgroundColor: '#4338ca',
                    transform: 'scale(1.05)',
                  },
                  transition: 'all 0.3s ease-in-out',
                }}
              >
                Login
              </Button>
            </>
          )}
        </Box>
      </Paper>
    </Container>
  );
};

export default Login;
