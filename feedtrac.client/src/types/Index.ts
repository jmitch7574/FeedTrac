// Type definitions for the client side of the FeedTrac application
// student related endpoints

// student register request type
export type studentRegister = {
  UserName: string;
  Email: string;
  FirstName: string;
  LastName: string;
  Password: string;
};
// student login request type
export type studentLogin = {
  Email: string;
  Password: string;
};

// password related endpoints
// forgot password request type
export type ForgotPasswordRequest = {
  email: string;
};
// code response for forgot password
export type ForgotPasswordResponse = {
  code: string;
};
// reset password request type
export type ResetPasswordRequest = {
  email: string;
  resetCode: string;
  newPassword: string;
};

// token refresh request and response types
export type RefreshRequest = {
  refreshToken: string;
};

export type RefreshResponse = {
  token: string;
};
