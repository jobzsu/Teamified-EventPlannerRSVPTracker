import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import AppHttpClient from '../utils/AppHttpClient';
import ToastifyHelper from '../utils/ToastifyHelper';

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
  CircularProgress,
} from '@mui/material';

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

const PublicEvents = () => {
  const navigate = useNavigate();

  const [events, setEvents] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [username, setUsername] = useState('');

  const fetchEvents = () => {
    setIsLoading(true);

    AppHttpClient.get('events/public')
      .then((response) => {
        if (response.data.isSuccess) {
          const eventsWithRsvpStatus = response.data.data.map(event => ({
            ...event,
            isRsvp: false
          }));
          setEvents(eventsWithRsvpStatus);
        } else {
          ToastifyHelper.AlertErrorNotif(response.data.message, 'fetchEventsError');
        }
      })
      .catch((error) => {
        ToastifyHelper.AlertErrorNotif(error.response?.data?.message || 'Failed to load public events.', 'fetchEventsError');
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  useEffect(() => {
    const storedUsername = sessionStorage.getItem('username');
    if (storedUsername) {
      setUsername(storedUsername);
    } else {
      setUsername('');
    }
    fetchEvents();
  }, []);

  const handleLoginClick = () => {
    navigate('/login');
  };

  const handleRsvp = (eventId) => {
    if (!username) {
      ToastifyHelper.AlertErrorNotif('Please log in to RSVP for an event.', 'rsvpLoginRequired');
      return;
    }

    let isSuccess = false;
    let errMssg = '';

		setIsLoading(true);

    // Make the API call to RSVP
    AppHttpClient.post(`/events/rsvp/${eventId}`)
      .then((response) => {
        isSuccess = response.data.isSuccess;
				if(!isSuccess)
						errMssg = response.data.message;
				else {
					// Update the specific event in state to reflect RSVP'd status and new reservedPax
          setEvents(prevEvents =>
            prevEvents.map(event =>
              event.id === eventId ? {
                ...event,
                isRsvp: true, // Set to true on successful RSVP
                reservedPax: response.data.data?.reservedPax || event.reservedPax + 1 // Update reservedPax from API response or increment
              } : event
            )
          );
				}
      })
      .catch((error) => {
        ToastifyHelper.AlertErrorNotif(error.response?.data?.message || 'Failed to RSVP.', 'rsvpError');
      })
			.finally(() => {
				setIsLoading(false);
				if(isSuccess)
					ToastifyHelper.AlertSuccessNotif('RSVP successful!', 'rsvpSuccess');
				else
					ToastifyHelper.AlertErrorNotif(errMssg, 'rsvpError');
			});
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
        backgroundColor: '#f3f4f6',
        p: 4,
      }}
    >
      <Box sx={{ width: '100%', display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 4 }}>
        <Typography component="h1" variant="h3" sx={{ fontWeight: 'bold', color: '#1f2937' }}>
          Public Events
        </Typography>
        {!username && (
          <Button
            variant="contained"
            onClick={handleLoginClick}
            sx={{
              backgroundColor: '#1976d2',
              '&:hover': { backgroundColor: '#1565c0' },
            }}
          >
            Log In
          </Button>
        )}
      </Box>

      <Paper elevation={6} sx={{ p: 4, width: '100%', borderRadius: 3 }}>
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
                      Description: {event.description || 'N/A'}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      RSVPs: {event.reservedPax} / {event.maxPax} Max
                    </Typography>
                  </CardContent>
                  <CardActions sx={{ justifyContent: 'flex-end', p: 2 }}>
                    <Button
                      size="small"
                      variant="contained"
                      onClick={() => handleRsvp(event.id)}
                      disabled={event.isRsvp}
                      sx={{
                        backgroundColor: event.isRsvp ? '#bdbdbd' : '#f57c00',
                        '&:hover': {
                          backgroundColor: event.isRsvp ? '#9e9e9e' : '#ef6c00',
                        },
                      }}
                    >
                      {event.isRsvp ? 'RSVP\'d!' : 'RSVP'}
                    </Button>
                  </CardActions>
                </Card>
              </Grid>
            ))
          ) : (
            <Grid item xs={12}>
              <Typography variant="h6" color="text.secondary" sx={{ textAlign: 'center', mt: 4 }}>
                No public events available at the moment.
              </Typography>
            </Grid>
          )}
        </Grid>
      </Paper>
    </Container>
  );
};

export default PublicEvents;
