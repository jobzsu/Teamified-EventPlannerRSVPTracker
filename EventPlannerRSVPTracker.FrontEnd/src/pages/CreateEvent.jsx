import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import AppHttpClient from '../utils/AppHttpClient';
import ToastifyHelper from '../utils/ToastifyHelper';

import {
  Box,
  TextField,
  Button,
  Typography,
  Container,
  Paper,
  CircularProgress,
} from '@mui/material';

const LoadingSpinner = () => {
  return (
    <Box 
      sx={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        height: '200px' // Adjust the height as needed
      }}
    >
      <CircularProgress />
    </Box>
  );
};

const CreateEvent = () => {
  const navigate = useNavigate();

  // State for form fields
  const [eventName, setEventName] = useState('');
  const [date, setDate] = useState('');
  const [time, setTime] = useState('');
  const [location, setLocation] = useState('');
  const [description, setDescription] = useState('');
  const [maxRsvps, setMaxRsvps] = useState('');

  // State for form validation errors
  const [eventNameError, setEventNameError] = useState(false);
  const [dateError, setDateError] = useState(false);
  const [timeError, setTimeError] = useState(false);
  const [locationError, setLocationError] = useState(false);
  const [maxRsvpsError, setMaxRsvpsError] = useState(false);

  // Loading and error states for API calls
  const [isLoading, setIsLoading] = useState(false);

  // Username from sessionStorage for display
  const [username, setUsername] = useState('');

  useEffect(() => {
    const storedUsername = sessionStorage.getItem('username');
    if (storedUsername) {
      setUsername(storedUsername);
    } else {
      navigate('/login');
      return;
    }
  }, []);

  const validateForm = () => {
    let isValid = true;

    // Event Name validation
    if (!eventName.trim()) {
      setEventNameError(true);
      isValid = false;
    } else {
      setEventNameError(false);
    }

    // Date validation
    if (!date) {
      setDateError(true);
      isValid = false;
    } else {
      setDateError(false);
    }

    // Time validation
    if (!time) {
      setTimeError(true);
      isValid = false;
    } else {
      setTimeError(false);
    }

    // Location validation
    if (!location.trim()) {
      setLocationError(true);
      isValid = false;
    } else {
      setLocationError(false);
    }

    // Max RSVPs validation
    const parsedMaxRsvps = parseInt(maxRsvps, 10);
    if (isNaN(parsedMaxRsvps) || parsedMaxRsvps <= 0) {
      setMaxRsvpsError(true);
      isValid = false;
    } else {
      setMaxRsvpsError(false);
    }

    return isValid;
  };

  /**
   * Handles the save event action (create new event).
   */
  const handleSave = () => {
    if (!validateForm()) {
      return; // Stop if validation fails
    }

    setIsLoading(true);

    const newEvent = {
      name: eventName.trim(),
      date: date,
      time: time,
      location: location.trim(), // Location can be empty
      description: description.trim(), // Description can be empty
      maxPax: parseInt(maxRsvps, 10),
      host: username
    };

    let isSuccess = false;
    let errMssg = '';
    
    AppHttpClient.post('/events', newEvent )
      .then((response) => {
        isSuccess = response.data.isSuccess;
        if(!isSuccess)
            errMssg = response.data.message;
      })
      .catch((error) => {
        errMssg = error.response.data.message;
      })
      .finally(() => {
        if(isSuccess) {
          ToastifyHelper.AlertSuccessNotif('Event created successfully!', 'eventCreateSuccess');
          setTimeout(() => {
            navigate('/my-events');
          }, 2000);
        }
        else {
          setIsLoading(false);
          ToastifyHelper.AlertErrorNotif(errMssg, 'eventCreateError');
        }
      });
  };

  /**
   * Handles the cancel action, navigating back to the events list.
   */
  const handleCancel = () => {
    navigate('/my-events'); // Navigate back to the events list
  };

  // Conditional rendering for loading and error states
  if (isLoading) {
    return (
      <Container
        component="main"
        maxWidth="sm"
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          minHeight: '100vh',
          backgroundColor: '#f3f4f6',
          p: 4,
        }}
      >
        <LoadingSpinner />
      </Container>
    );
  }

  return (
    <Container
      component="main"
      maxWidth="sm"
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        minHeight: '100vh',
        backgroundColor: '#f3f4f6', // Light gray background
        p: 4,
      }}
    >
      <Paper elevation={12} sx={{ p: 4, width: '100%', borderRadius: 3 }}>
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
          }}
        >
          <Typography component="h1" variant="h3" sx={{ mb: 1, fontWeight: 'bold', color: '#1f2937' }}>
            Event Planner
          </Typography>
          <Typography component="h2" variant="h4" sx={{ mb: 2, fontWeight: 'bold', color: '#333' }}>
            Event Creation
          </Typography>
          <Typography variant="subtitle1" color="text.secondary" sx={{ mt: 1, mb: 3 }}>
            Logged in as: <span style={{ fontWeight: 'bold' }}>{username || 'Guest'}</span>
          </Typography>

          <Box component="form" sx={{ display: 'flex', flexDirection: 'column', gap: 2, width: '100%' }}>
            <TextField
              label="Event Name"
              fullWidth
              value={eventName}
              onChange={(e) => {
                setEventName(e.target.value);
                if (eventNameError) setEventNameError(false); // Clear error on change
              }}
              margin="normal"
              variant="outlined"
              required
              error={eventNameError}
              helperText={eventNameError && "Event Name is required"}
            />
            <Box sx={{ display: 'flex', gap: 2, width: '100%' }}>
              <TextField
                label="Date"
                type="date"
                fullWidth
                value={date}
                onChange={(e) => {
                  setDate(e.target.value);
                  if (dateError) setDateError(false); // Clear error on change
                }}
                margin="normal"
                variant="outlined"
                InputLabelProps={{ shrink: true }}
                required
                error={dateError}
                helperText={dateError && "Date is required"}
              />
              <TextField
                label="Time"
                type="time"
                fullWidth
                value={time}
                onChange={(e) => {
                  setTime(e.target.value);
                  if (timeError) setTimeError(false); // Clear error on change
                }}
                margin="normal"
                variant="outlined"
                InputLabelProps={{ shrink: true }}
                required
                error={timeError}
                helperText={timeError && "Time is required"}
              />
            </Box>
            <TextField
              label="Location"
              fullWidth
              value={location}
              onChange={(e) => setLocation(e.target.value)}
              margin="normal"
              variant="outlined"
              required
              error={locationError}
              helperText={locationError && "Location is required"}
            />
            <TextField
              label="Description"
              fullWidth
              multiline
              rows={4}
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              margin="normal"
              variant="outlined"
            />
            <TextField
              label="Max RSVPs"
              type="number"
              fullWidth
              value={maxRsvps}
              onChange={(e) => {
                setMaxRsvps(e.target.value);
                if (maxRsvpsError) setMaxRsvpsError(false); // Clear error on change
              }}
              margin="normal"
              variant="outlined"
              required
              error={maxRsvpsError}
              helperText={maxRsvpsError && "Max RSVPs is required and must be a positive number"}
              inputProps={{ min: 0 }}
            />
          </Box>
        </Box>
        <Box sx={{ mt: 4, display: 'flex', justifyContent: 'center', gap: 2 }}>
          <Button
            variant="contained"
            onClick={handleSave}
            disabled={isLoading}
            sx={{
              minWidth: 120,
              py: 1.2,
              backgroundColor: '#4caf50',
              '&:hover': { backgroundColor: '#388e3c' },
            }}
          >
            {isLoading ? <CircularProgress size={24} color="inherit" /> : 'Create Event'}
          </Button>
          <Button
            variant="outlined"
            onClick={handleCancel}
            disabled={isLoading}
            sx={{
              minWidth: 120,
              py: 1.2,
              ml: 2,
              borderColor: '#757575',
              color: '#757575',
              '&:hover': { borderColor: '#424242', color: '#424242' },
            }}
          >
            Cancel
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

export default CreateEvent;