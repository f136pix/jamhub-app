require 'bunny'

begin
  $rabbitmq_conn = Bunny.new(hostname: 'localhost', username: 'guest', password: 'guest', port: 5672)
  $rabbitmq_conn.start

rescue Bunny::TCPConnectionFailed => e
  puts "--> Connection to RabbitMQ failed"
rescue Bunny::PossibleAuthenticationFailureError => e
  puts "--> Could not authenticate to the queue"
end

# closes ampq conn
at_exit { $rabbitmq_conn.close if $rabbitmq_conn }