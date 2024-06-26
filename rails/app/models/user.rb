class User < ApplicationRecord

  # info added on the jwt payload
  def jwt_payload
    {
      'email' => email,
    }
  end

  # Include default devise modules. Others available are:
  # :confirmable, :lockable, :timeoutable, :trackable and :omniauthable
  devise :database_authenticatable,
         :confirmable,
         :jwt_authenticatable,
         :registerable,
         jwt_revocation_strategy: JwtDenylist # uses the JwtDenylist class to check for revoked tokens

end
