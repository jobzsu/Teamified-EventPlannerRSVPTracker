import React from 'react';
import { Box, CircularProgress } from '@mui/material';

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

export default LoadingSpinner;
