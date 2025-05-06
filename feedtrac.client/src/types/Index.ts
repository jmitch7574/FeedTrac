// Type definitions for the client side of the FeedTrac application
// student related endpoints

// student register request type
export type studentRegister = {
  // UserName: string;
  Email: string;
  FirstName: string;
  LastName: string;
  Password: string;
};
// student login request type
export type studentLogin = {
  Email: string;
  Password: string;
  rememberMe: boolean;
};

// teacher related endpoints
export type teacherRegister = {
  // UserName: string;
  Email: string;
  FirstName: string;
  LastName: string;
};
// student login request type
export type teacherLogin = {
  Email: string;
  Password: string;
  twoFactorCode: string;
  rememberMe: boolean;
};

// auth response type
export type AuthResponse = {
  token: string;
  refreshToken: string;
  user: {
    email: string;
    firstName: string;
    lastName: string;
  };
};

export type IdentityResponse = {
  status: string;
  userInfo: {
    userId: string;
    firstName: string;
    lastName: string;
    email: string;
  };
};

// modules related types
export type Module = {
  id: number;
  name: moduleName;
  joinCode: string;
};

export type teacherModuleId = {
  teacherId: string;
};

export type teacherModuleEmail = {
  teacherId: string;
};

export type moduleName = {
  moduleName: string;
};

export type ModuleResponse = {
  modules: Module[];
};

// user related types
export type PublicUser = {
  id: string;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  emailConfirmed: boolean;
  twoFactorEnabled: boolean;
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
export type ForgotPasswordFollowupRequest = {
  email: string;
  resetCode: string;
  newPassword: string;
};

export type ResetPasswordRequest = {
  currentPassword: string;
  newPassword: string;
}

// token refresh request and response types
export type RefreshRequest = {
  refreshToken: string;
};

export type RefreshResponse = {
  token: string;
};

// ticket related types

export interface TicketMessage {
  id: number;
  senderId: string;
  senderName: string;
  content: string;
  createdAt: string;
  imageIds: number[];
}

export interface Ticket {
  id: number;
  title: string;
  messages: TicketMessage[];
  createdAt: string;
  updatedAt: string;
  status: number;
}

export interface TicketResponse {
  tickets: Ticket[];
}

export interface CreateTicketFormData {
  title: string;
  content: string;
  images?: File[];
}

export interface AddMessageFormData {
  content: string;
  images?: File[];
}
