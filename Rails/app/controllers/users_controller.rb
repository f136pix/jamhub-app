class UsersController < ApplicationController
  
  # GET /users
  def index
    puts"teste"
    render json: User.all
  end
end