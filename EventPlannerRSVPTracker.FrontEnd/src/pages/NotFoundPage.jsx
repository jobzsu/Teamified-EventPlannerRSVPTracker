import {
  Container,
  CssBaseline,
  Grid,
  Paper,
  Stack,
  styled,
} from "@mui/material";

const NotFoundPage = () => {
  const Item = styled(Paper)(({ theme }) => ({
    backgroundColor: theme.palette.mode === "dark" ? "#1A2027" : "#fff",
    ...theme.typography.body2,
    padding: theme.spacing(1),
    textAlign: "center",
    color: theme.palette.text.secondary,
  }));

  return (
    <>
      <CssBaseline />
      <Stack direction="row">
        <Item>
          <img
            src="/images/catroomguardian.jfif"
            alt="img"
            style={{ width: "270px", height: "270px" }}
          />
        </Item>
      </Stack>
      <Container maxWidth="lg"></Container>
    </>
  );
};

export default NotFoundPage;
