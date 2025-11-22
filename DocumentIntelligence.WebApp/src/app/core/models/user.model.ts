export interface UserResponse {
  id: string;
  email: string;
  name: string;
  roles: string[];
  accessToken: string;
  refreshToken: string;
}

export interface UserRegistrationResponse {
  id: string;
  email: string;
  name: string;
}
