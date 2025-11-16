export interface UserResponse {
  id: string;
  email: string;
  name: string;
  roles: string[];
  accessToken: string;
  refreshToken: string;
}
