import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import NotFoundPage from "../pages/NotFoundPage";
import HomePage from "../pages/HomePage";
import Login from "../pages/Login";
import UserEvents from "../pages/UserEvents";
import CreateEvent from "../pages/CreateEvent";
import EditEvent from "../pages/EditEvent";
import PublicEvents from "../pages/PublicEvents";

const AppRoutes = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Navigate to="/login" />} />
        <Route path="/home" element={<HomePage />} />
        <Route path="/login" element={<Login />} />
        <Route path="/my-events" element={<UserEvents />} />
        <Route path="/create-event" element={<CreateEvent />} />
        <Route path="/edit-event/:eventId" element={<EditEvent />} />
        <Route path="/public-events" element={<PublicEvents />} />
        <Route path="/*" element={<NotFoundPage />} />
      </Routes>
    </Router>
  );
};

export default AppRoutes;
