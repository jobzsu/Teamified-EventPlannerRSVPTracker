import React, { useState, useEffect } from 'react';
import axios from 'axios'; // Import Axios
import {
  Box,
  Button,
  Typography,
  Container,
  Paper,
  Grid,
  Card,
  CardContent,
  CardActions,
  CircularProgress, // Import CircularProgress for the spinner
} from '@mui/material';
import AppHttpClient from '../utils/AppHttpClient';
import ToastifyHelper from '../utils/ToastifyHelper';
import { useNavigate } from "react-router-dom";

/**
 * A reusable component to display a centered loading spinner.
 * It uses Material-UI's CircularProgress component.
 */
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

const UserEvents = () => {
  const [events, setEvents] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
	const [username, setUsername] = useState('');

	const navigate = useNavigate();

  // useEffect hook to fetch events data when the component mounts.
  useEffect(() => {

		// Check if has user session
		const storedUsername = sessionStorage.getItem('username');
		console.log('Stored username:', storedUsername);
    if (storedUsername) {
      setUsername(storedUsername);
    } else {
      navigate('/login');
		}

    setIsLoading(true); 

		let isSuccess = false;
    let errMssg = '';

    AppHttpClient.get(`/events/userevents/${storedUsername}`)
      .then((response) => {
        isSuccess = response.data.isSuccess;
        if(!isSuccess)
            errMssg = response.data.message;
        else {
          setEvents(response.data.data);
        }
      })
      .catch((error) => {
        errMssg = error.response.data.message;
      })
      .finally(() => {
				setIsLoading(false);
        if(!isSuccess)
          ToastifyHelper.AlertErrorNotif(errMssg, 'loginError');
      });
  }, []);

  const handleCreateNewEvent = () => {
		if(isLoading) return;
    navigate('/create-event');
  };

  const handleEditEvent = (eventId) => {
		if(isLoading) return;
		navigate(`/edit-event/${eventId}`);
  };

  const handleDeleteEvent = async (eventId) => {
    console.log(`Delete event with ID: ${eventId}`);
    
		let isSuccess = false;
    let errMssg = '';

		AppHttpClient.delete(`/events/${eventId}`)
      .then((response) => {
        isSuccess = response.data.isSuccess;
        if(!isSuccess)
            errMssg = response.data.message;
      })
      .catch((error) => {
        errMssg = error.response.data.message;
      })
      .finally(() => {
				setIsLoading(false);
        if(!isSuccess)
          ToastifyHelper.AlertErrorNotif(errMssg, 'loginError');
				else {
					ToastifyHelper.AlertSuccessNotif('Event deleted successfully!', `deleteSuccess-${eventId}`);
					setEvents(events.filter((event) => event.id !== eventId));
				}
      });
  };

  const handleGoToPublicEvents = () => {
		if(isLoading) return;
    navigate('/public-events');
  };

  if (isLoading) {
    return (
      <Container
        component="main"
        maxWidth="lg"
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
      maxWidth="lg"
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        minHeight: '100vh',
        backgroundColor: '#f3f4f6', // Light gray background
        p: 4,
      }}
    >
      <Paper elevation={6} sx={{ p: 4, width: '100%', borderRadius: 3, mb: 4 }}>
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            mb: 4,
          }}
        >
          {/* Main heading for the application */}
          <Typography component="h1" variant="h3" sx={{ mb: 1, fontWeight: 'bold', color: '#1f2937' }}>
            Event Planner
          </Typography>
          {/* Logged in user information */}
          <Typography variant="h6" sx={{ mb: 3, color: '#555' }}>
            Logged in as: <span style={{ fontWeight: 'bold' }}>{username}</span>
          </Typography>

          {/* Create New Event Button */}
          <Button
            variant="contained"
            onClick={handleCreateNewEvent}
            sx={{
              mt: 2,
              py: 1.5,
              px: 4,
              fontWeight: 'bold',
              borderRadius: 2,
              backgroundColor: '#4caf50', // Green for create
              '&:hover': {
                backgroundColor: '#388e3c',
              },
            }}
          >
            Create New Event
          </Button>
        </Box>

        {/* My Events Section */}
        <Typography component="h2" variant="h4" sx={{ mb: 3, fontWeight: 'bold', color: '#333' }}>
          My Events
        </Typography>

        <Grid container spacing={3}>
          {events.length > 0 ? (
            events.map((event) => (
              <Grid item xs={12} sm={6} md={4} lg={3} key={event.id}>
                <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column', borderRadius: 2, boxShadow: 3 }}>
                  <CardContent sx={{ flexGrow: 1 }}>
                    <Typography gutterBottom variant="h6" component="div" sx={{ fontWeight: 'bold' }}>
                      {event.name}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Date: {event.date}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Time: {event.time}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Location: {event.location}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      RSVPs: {event.reservedPax} / {event.maxPax} Max
                    </Typography>
                  </CardContent>
                  <CardActions sx={{ justifyContent: 'flex-end', p: 2 }}>
                    <Button size="small" variant="outlined" onClick={() => handleEditEvent(event.id)}>
                      Edit
                    </Button>
                    <Button size="small" variant="contained" color="error" onClick={() => handleDeleteEvent(event.id)}>
                      Delete
                    </Button>
                  </CardActions>
                </Card>
              </Grid>
            ))
          ) : (
            <Grid item xs={12}>
              <Typography variant="h6" color="text.secondary" sx={{ textAlign: 'center', mt: 4 }}>
                No events found. Create a new event!
              </Typography>
            </Grid>
          )}
        </Grid>
      </Paper>

      {/* Link to Public Events Page */}
      <Box sx={{ mt: 4 }}>
        <Button
          variant="text"
          onClick={handleGoToPublicEvents}
          sx={{
            color: '#1976d2', // Blue for link
            fontWeight: 'bold',
            '&:hover': {
              textDecoration: 'underline',
            },
          }}
        >
          Link to Public Events Page
        </Button>
      </Box>
    </Container>
  );
};

export default UserEvents;
